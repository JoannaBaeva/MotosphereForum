using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Web.Models.Marketplace;
using System.Security.Claims;

namespace MotorcycleForum.Web.Controllers
{
    public class MarketplaceController : Controller
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly S3Service _s3Service;

        public MarketplaceController(MotorcycleForumDbContext context, S3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        // GET: MarketplaceListings
        [Authorize]
        public async Task<IActionResult> Index(
            string? searchTerm,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortOption,
            int pageNumber = 1)
        {
            const int pageSize = 9;

            var listingsQuery = _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Images)
                .Where(l => l.IsActive)
                .AsQueryable();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            return View(model);
        }

        // GET: MarketplaceListings/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var listing = await _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Seller)
                    .ThenInclude(s => s.MarketplaceListings.Where(m => m.IsActive))
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }


        [Authorize]
        public async Task<IActionResult> MyListings()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var listings = await _context.MarketplaceListings
                .Include(l => l.Images)
                .Include(l => l.Category)
                .Where(l => l.SellerId == userId)
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();

            return View(listings);
        }


        // GET: MarketplaceListings/Create
        [Authorize]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();

            var model = new MarketplaceListingViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "Name")
            };

            return View(model);
        }


        // POST: MarketplaceListings/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MarketplaceListingViewModel model)
        {
            if (model.ImageFiles == null || !model.ImageFiles.Any(f => f.Length > 0))
            {
                ModelState.AddModelError("ImageFiles", "Please upload at least one image.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            var sellerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
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

            // Upload images to S3
            if (model.ImageFiles is not null && model.ImageFiles.Count > 0)
            {
                var s3 = new S3Service();

                foreach (var formFile in model.ImageFiles)
                {
                    if (formFile.Length > 0)
                    {
                        var fileExt = Path.GetExtension(formFile.FileName);
                        var s3FileName = $"marketplace/{listingId}/{Guid.NewGuid()}{fileExt}";

                        using var stream = formFile.OpenReadStream();
                        var imageUrl = await s3.UploadFileAsync(stream, s3FileName);

                        listing.Images.Add(new MarketplaceListingImage
                        {
                            ImageId = Guid.NewGuid(),
                            ListingId = listingId,
                            ImageUrl = imageUrl
                        });
                    }
                }
            }


            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyListings));
        }

        // GET: MarketplaceListings/Edit/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null)
                return NotFound();
            if (listing.SellerId != userId)
                return Forbid();

            var model = new MarketplaceListingViewModel
            {
                ListingId = listing.ListingId,
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                Location = listing.Location,
                CategoryId = listing.CategoryId,
                IsActive = listing.IsActive,
                PhoneNumber = listing.SellerPhoneNumber,
                Categories = new SelectList(_context.Categories, "CategoryId", "N   ", listing.CategoryId),
                ExistingImageUrls = listing.Images.Select(i => i.ImageUrl).ToList(),
                ImageIds = listing.Images.Select(i => i.ImageId).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarketplaceListingViewModel model)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == model.ListingId);

            if (listing == null)
                return NotFound();
            if (listing.SellerId != userId)
                return Forbid();

            // Count how many images are already present and will remain
            int existingImagesCount = listing.Images.Count;
            int toDeleteCount = model.ImagesToDelete?.Count ?? 0;
            int remainingImages = existingImagesCount - toDeleteCount;
            int newUploads = model.ImageFiles?.Count ?? 0;

            if ((remainingImages + newUploads) < 1)
            {
                ModelState.AddModelError("", "You must have at least one image in your listing.");
                model.Categories = new SelectList(_context.Categories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            // Update the listing fields
            listing.Title = model.Title;
            listing.Description = model.Description;
            listing.Price = model.Price;
            listing.Location = model.Location;
            listing.CategoryId = model.CategoryId;
            listing.IsActive = model.IsActive;
            listing.SellerPhoneNumber = model.PhoneNumber;

            // Update images if new ones were uploaded
            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                var s3 = new S3Service();

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        var fileKey = $"marketplace/{model.ListingId}/{Guid.NewGuid()}{fileExt}";

                        using var stream = file.OpenReadStream();
                        var imageUrl = await s3.UploadFileAsync(stream, fileKey);

                        await _context.MarketplaceListingImages.AddAsync(new MarketplaceListingImage
                        {
                            ImageId = Guid.NewGuid(),
                            ListingId = listing.ListingId,
                            ImageUrl = imageUrl
                        });

                    }
                }

            }

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    Console.WriteLine($"Concurrency conflict on entity: {entry.Entity.GetType().Name}");
                }

                throw;
            }

            // Redirect back to Details view
            return RedirectToAction(nameof(Details), new { id = listing.ListingId });
        }

        // GET: MarketplaceListings/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var listing = await _context.MarketplaceListings
                .Include(l => l.Category)
                .Include(l => l.Seller)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null)
                return NotFound();
            if (listing.SellerId != userId)
                return Forbid();

            return View(listing);
        }


        // POST: MarketplaceListings/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == id);

            if (listing == null)
                return NotFound();

            var s3 = new S3Service();

            // Delete all S3 images
            foreach (var image in listing.Images)
            {
                var s3Key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
                await s3.DeleteFileAsync(s3Key);
            }

            _context.MarketplaceListings.Remove(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyListings));
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages(IFormFileCollection files, Guid listingId)
        {
            if (files.Count > 5)
                return BadRequest("You can upload up to 5 images.");

            var listing = await _context.MarketplaceListings.FindAsync(listingId);
            if (listing == null)
                return NotFound("Listing not found.");

            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";

                using (var stream = file.OpenReadStream())  // Use OpenReadStream directly
                {
                    var imageUrl = await _s3Service.UploadFileAsync(stream, fileName);
                    imageUrls.Add(imageUrl);

                    _context.MarketplaceListingImages.Add(new MarketplaceListingImage
                    {
                        ImageId = Guid.NewGuid(),
                        ImageUrl = imageUrl,
                        ListingId = listingId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(imageUrls);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(Guid imageId, Guid listingId)
        {
            var image = await _context.MarketplaceListingImages.FirstOrDefaultAsync(i => i.ImageId == imageId);
            if (image == null)
                return NotFound();

            var s3 = new S3Service();
            var s3Key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
            await s3.DeleteFileAsync(s3Key);

            _context.MarketplaceListingImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = listingId });
        }

    }
}
