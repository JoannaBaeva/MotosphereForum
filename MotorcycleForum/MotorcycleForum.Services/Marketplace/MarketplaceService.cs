using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Services.Models.Marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MotorcycleForum.Data.Entities.Marketplace;
using Microsoft.AspNetCore.Http;

namespace MotorcycleForum.Services.Marketplace
{
    public class MarketplaceService : IMarketplaceService
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly S3Service _s3Service;

        public MarketplaceService(MotorcycleForumDbContext context, S3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }
        
        public async Task<MarketplaceFilterViewModel> GetFilteredListingsAsync(
            string? searchTerm,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
        string? sortOption,
            int pageNumber,
            ClaimsPrincipal user)
        {
            const int pageSize = 9;

            var listingsQuery = _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Images)
                .Where(l => l.IsActive)
                .AsQueryable();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            listingsQuery = listingsQuery.Where(l => l.SellerId.ToString() != userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                listingsQuery = listingsQuery.Where(l =>
                    l.Title.Contains(searchTerm) || l.Description.Contains(searchTerm));
            }

            if (categoryId.HasValue && categoryId != Guid.Empty)
            {
                listingsQuery = listingsQuery.Where(l => l.CategoryId == categoryId);
            }

            if (minPrice.HasValue)
            {
                listingsQuery = listingsQuery.Where(l => l.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                listingsQuery = listingsQuery.Where(l => l.Price <= maxPrice);
            }

            listingsQuery = sortOption switch
            {
                "price-asc" => listingsQuery.OrderBy(l => l.Price),
                "price-desc" => listingsQuery.OrderByDescending(l => l.Price),
                _ => listingsQuery.OrderByDescending(l => l.CreatedDate), // default: newest first
            };

            int totalListings = await listingsQuery.CountAsync();

            var pagedListings = await listingsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new MarketplaceFilterViewModel
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOption = sortOption,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalListings / (double)pageSize),
                Categories = await _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryId.ToString()
                    }).ToListAsync(),
                Listings = pagedListings
            };

            return model;
        }

        public async Task<MarketplaceListing?> GetListingDetailsAsync(Guid id)
        {
            return await _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Seller)
                .ThenInclude(s => s.MarketplaceListings.Where(m => m.IsActive))
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);
        }
        
        public async Task<List<MarketplaceListing>> GetUserListingsAsync(ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _context.MarketplaceListings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Where(l => l.SellerId == userId)
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();
        }
        
        public async Task<MarketplaceListingViewModel> GetCreateViewModelAsync()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return new MarketplaceListingViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "Name")
            };
        }
        
        public async Task<Guid?> CreateListingAsync(MarketplaceListingViewModel model, ClaimsPrincipal user)
        {
            if (model.ImageFiles == null || !model.ImageFiles.Any(f => f.Length > 0))
                return null;

            var sellerId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var listingId = Guid.NewGuid();

            var listing = new MarketplaceListing(sellerId)
            {
                ListingId = listingId,
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Location = model.Location,
                CategoryId = model.CategoryId,
                CreatedDate = DateTime.UtcNow,
                IsActive = model.IsActive,
                SellerPhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? "No phone number given" : model.PhoneNumber
            };

            foreach (var formFile in model.ImageFiles)
            {
                if (formFile.Length > 0)
                {
                    var fileExt = Path.GetExtension(formFile.FileName);
                    var s3FileName = $"marketplace/{listingId}/{Guid.NewGuid()}{fileExt}";

                    using var stream = formFile.OpenReadStream();
                    var imageUrl = await _s3Service.UploadFileAsync(stream, s3FileName);

                    listing.Images.Add(new MarketplaceListingImage
                    {
                        ImageId = Guid.NewGuid(),
                        ListingId = listingId,
                        ImageUrl = imageUrl
                    });
                }
            }

            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            return listingId;
        }

        public async Task<MarketplaceListingViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null || listing.SellerId != userId)
                return null;

            return new MarketplaceListingViewModel
            {
                ListingId = listing.ListingId,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId,
                IsActive = listing.IsActive,
                PhoneNumber = listing.SellerPhoneNumber,
                Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", listing.CategoryId),
                ExistingImageUrls = listing.Images.Select(i => i.ImageUrl).ToList(),
                ImageIds = listing.Images.Select(i => i.ImageId).ToList()
            };
        }

        public async Task<bool> UpdateListingAsync(MarketplaceListingViewModel model, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == model.ListingId);

            if (listing == null || listing.SellerId != userId)
                return false;

            int existingImagesCount = listing.Images.Count;
            int toDeleteCount = model.ImagesToDelete?.Count ?? 0;
            int remainingImages = existingImagesCount - toDeleteCount;
            int newUploads = model.ImageFiles?.Count ?? 0;

            if ((remainingImages + newUploads) < 1)
                return false;

            // Update basic fields
            listing.Title = model.Title;
            listing.Description = model.Description;
            listing.Price = model.Price;
            listing.Location = model.Location;
            listing.CategoryId = model.CategoryId;
            listing.IsActive = model.IsActive;
            listing.SellerPhoneNumber = model.PhoneNumber;

            // Delete marked images
            if (model.ImagesToDelete != null && model.ImagesToDelete.Any())
            {
                foreach (var imageId in model.ImagesToDelete)
                {
                    var image = listing.Images.FirstOrDefault(i => i.ImageId == imageId);
                    if (image != null)
                    {
                        var s3Key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
                        await _s3Service.DeleteFileAsync(s3Key);
                        _context.MarketplaceListingImages.Remove(image);
                    }
                }
            }

            // Upload new images
            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        var key = $"marketplace/{model.ListingId}/{Guid.NewGuid()}{fileExt}";

                        using var stream = file.OpenReadStream();
                        var url = await _s3Service.UploadFileAsync(stream, key);

                        await _context.MarketplaceListingImages.AddAsync(new MarketplaceListingImage
                        {
                            ImageId = Guid.NewGuid(),
                            ListingId = listing.ListingId,
                            ImageUrl = url
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<MarketplaceListing?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var listing = await _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Seller)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null || listing.SellerId != userId)
                return null;

            return listing;
        }
        
        public async Task<bool> DeleteListingAsync(Guid id)
        {
            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null)
                return false;

            foreach (var image in listing.Images)
            {
                var s3Key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
                await _s3Service.DeleteFileAsync(s3Key);
            }

            _context.MarketplaceListings.Remove(listing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> UploadImagesAsync(IFormFileCollection files, Guid listingId)
        {
            var listing = await _context.MarketplaceListings.FindAsync(listingId);
            if (listing == null)
                return new List<string>();

            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length <= 0) continue;

                var fileName = $"marketplace/{listingId}/{Guid.NewGuid()}_{file.FileName}";
                using var stream = file.OpenReadStream();
                var imageUrl = await _s3Service.UploadFileAsync(stream, fileName);
                imageUrls.Add(imageUrl);

                _context.MarketplaceListingImages.Add(new MarketplaceListingImage
                {
                    ImageId = Guid.NewGuid(),
                    ImageUrl = imageUrl,
                    ListingId = listingId
                });
            }

            await _context.SaveChangesAsync();
            return imageUrls;
        }
        
        public async Task<bool> DeleteImageAsync(Guid imageId)
        {
            var image = await _context.MarketplaceListingImages.FirstOrDefaultAsync(i => i.ImageId == imageId);
            if (image == null)
                return false;

            var s3Key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
            await _s3Service.DeleteFileAsync(s3Key);

            _context.MarketplaceListingImages.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
