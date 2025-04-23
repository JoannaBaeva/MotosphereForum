using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Services.Models.Marketplace;
using System.Security.Claims;
using MotorcycleForum.Services.Marketplace;

namespace MotorcycleForum.Web.Controllers
{
    public class MarketplaceController : Controller
    {
        private readonly IMarketplaceService _marketplaceService;

        public MarketplaceController(IMarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
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
            var model = await _marketplaceService.GetFilteredListingsAsync(
                searchTerm, categoryId, minPrice, maxPrice, sortOption, pageNumber, User);

            return View(model);
        }


        // GET: MarketplaceListings/Details/{id}
        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            var listing = await _marketplaceService.GetListingDetailsAsync(id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }


        [Authorize]
        public async Task<IActionResult> MyListings()
        {
            var listings = await _marketplaceService.GetUserListingsAsync(User);
            return View(listings);
        }

        // GET: MarketplaceListings/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = await _marketplaceService.GetCreateViewModelAsync();
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
                model.Categories = await _marketplaceService.GetCreateViewModelAsync()
                    .ContinueWith(t => t.Result.Categories);
                return View(model);
            }

            var listingId = await _marketplaceService.CreateListingAsync(model, User);
            if (listingId == null)
            {
                ModelState.AddModelError("", "Failed to create listing.");
                model.Categories = await _marketplaceService.GetCreateViewModelAsync()
                    .ContinueWith(t => t.Result.Categories);
                return View(model);
            }

            return RedirectToAction(nameof(MyListings));
        }

        // GET: MarketplaceListings/Edit/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _marketplaceService.GetEditViewModelAsync(id, User);
            if (model == null)
                return NotFound();

            return View(model);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MarketplaceListingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _marketplaceService.GetCreateViewModelAsync()
                    .ContinueWith(t => t.Result.Categories);
                return View(model);
            }

            var success = await _marketplaceService.UpdateListingAsync(model, User);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to update the listing. Please check your input.");
                model.Categories = await _marketplaceService.GetCreateViewModelAsync()
                    .ContinueWith(t => t.Result.Categories);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id = model.ListingId });
        }


        // GET: MarketplaceListings/Delete/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var listing = await _marketplaceService.GetDeleteConfirmationAsync(id, User);

            if (listing == null)
                return NotFound();

            return View(listing);
        }



        // POST: MarketplaceListings/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _marketplaceService.DeleteListingAsync(id);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(MyListings));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages(IFormFileCollection files, Guid listingId)
        {
            if (files.Count > 5)
                return BadRequest("You can upload up to 5 images.");

            var result = await _marketplaceService.UploadImagesAsync(files, listingId);

            if (result == null || !result.Any())
                return NotFound("Listing not found or upload failed.");

            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(Guid imageId, Guid listingId)
        {
            var success = await _marketplaceService.DeleteImageAsync(imageId);

            if (!success)
                return NotFound();

            return RedirectToAction("Edit", new { id = listingId });
        }

    }
}