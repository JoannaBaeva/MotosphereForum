using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Services.Models.Admin;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Services.Admin;

namespace MotorcycleForum.Web.Controllers
{
    public class AdminController : Controller
    {
        IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Only Admins can access
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var model = await _adminService.GetDashboardStatsAsync();
            return View(model);
        }

        //Create Moderator
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateModerator()
        {
            var users = await _adminService.GetModeratorCandidatesAsync();

            var model = new CreateModeratorViewModel
            {
                Users = users
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Promote(Guid userId)
        {
            var success = await _adminService.PromoteToModeratorAsync(userId);

            if (!success)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(CreateModerator));
            }

            TempData["ModeratorCreated"] = "User has been promoted to Moderator!";
            return RedirectToAction(nameof(CreateModerator));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Demote(Guid userId)
        {
            var success = await _adminService.DemoteFromModeratorAsync(userId);

            if (!success)
                return NotFound();

            TempData["ModeratorDemoted"] = "User has been demoted.";
            return RedirectToAction(nameof(CreateModerator));
        }


        //Mods can access too

        // Mod Panel
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Mod()
        {
            var model = await _adminService.GetDashboardStatsAsync();
            return View(model);
        }

        // Create Marketplace Category

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateMarketplaceCategory()
        {
            var categories = await _adminService.GetMarketplaceCategoriesAsync();

            var model = new CreateMarketplaceCategoryViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMarketplaceCategory(CreateMarketplaceCategoryViewModel category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name is required.");
                category.Categories = await _adminService.GetMarketplaceCategoriesAsync(); // repopulate dropdown
                return View(category);
            }

            var success = await _adminService.CreateMarketplaceCategoryAsync(category.Name);

            if (success)
                TempData["MarketplaceCategoryCreated"] = "Marketplace category created successfully!";
            else
                TempData["Error"] = "Something went wrong while creating the category.";

            return RedirectToAction(nameof(CreateMarketplaceCategory));
        }


        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMarketplaceCategory(Guid id)
        {
            var success = await _adminService.DeleteMarketplaceCategoryAsync(id);

            if (success)
            {
                TempData["MarketplaceCategoryDeleted"] = "Marketplace category deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Could not delete category. It may not exist.";
            }

            return RedirectToAction(nameof(CreateMarketplaceCategory));
        }

        // Create Event Category

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateEventCategory()
        {
            var categories = await _adminService.GetEventCategoriesAsync();

            var model = new CreateEventCategoryViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEventCategory(CreateEventCategoryViewModel category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name is required.");
                return View(category);
            }

            var success = await _adminService.CreateEventCategoryAsync(category.Name);

            if (!success)
            {
                ModelState.AddModelError("", "Something went wrong while creating the category.");
                return View(category);
            }

            TempData["EventCategoryCreated"] = "Event category created successfully!";
            return RedirectToAction(nameof(CreateEventCategory));
        }


        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventCategory(Guid id)
        {
            var success = await _adminService.DeleteEventCategoryAsync(id);

            if (!success)
            {
                TempData["Error"] = "Event category could not be deleted or was not found.";
                return RedirectToAction(nameof(CreateEventCategory));
            }

            TempData["EventCategoryDeleted"] = "Event category deleted successfully!";
            return RedirectToAction(nameof(CreateEventCategory));
        }


        //Create Forum Topic

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateForumTopic()
        {
            var topics = await _adminService.GetForumTopicsAsync();
            var model = new CreateForumTopicViewModel { Topics = topics };
            return View(model);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForumTopic(CreateForumTopicViewModel topic)
        {
            if (string.IsNullOrWhiteSpace(topic.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                topic.Topics = await _adminService.GetForumTopicsAsync();
                return View(topic);
            }

            var success = await _adminService.CreateForumTopicAsync(topic.Name);

            if (success)
                TempData["ForumTopicCreated"] = "Forum topic created successfully!";

            return RedirectToAction(nameof(CreateForumTopic));
        }


        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForumTopic(int id)
        {
            var success = await _adminService.DeleteForumTopicAsync(id);

            if (!success)
                return NotFound();

            TempData["ForumTopicDeleted"] = "Forum topic deleted successfully!";
            return RedirectToAction(nameof(CreateForumTopic));
        }

        // Ban Users
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> BanUsers()
        {
            var currentUserEmail = User.Identity?.Name;
            var users = await _adminService.GetBanUsersListAsync(currentUserEmail);
            return View(users);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(string email)
        {
            var result = await _adminService.BanUserAsync(email);

            if (!result)
            {
                TempData["Error"] = "User not found.";
            }
            else
            {
                TempData["UserBanned"] = $"{email} has been banned.";
            }

            return RedirectToAction(nameof(BanUsers));
        }


        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnbanUser(string email)
        {
            var result = await _adminService.UnbanUserAsync(email);

            if (!result)
            {
                TempData["Error"] = "User not found.";
            }
            else
            {
                TempData["UserUnbanned"] = $"{email} has been unbanned.";
            }

            return RedirectToAction(nameof(BanUsers));
        }

        // Approve Events
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> ApproveEvents()
        {
            var pendingEvents = await _adminService.GetPendingEventsAsync();
            return View(pendingEvents);
        }
        
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            var success = await _adminService.ApproveEventAsync(id);

            if (!success)
            {
                TempData["Error"] = "Event not found.";
            }
            else
            {
                TempData["EventApproved"] = "The event was successfully approved!";
            }

            return RedirectToAction(nameof(ApproveEvents));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var success = await _adminService.RejectEventAsync(id);

            if (!success)
            {
                TempData["Error"] = "Event not found.";
            }
            else
            {
                TempData["EventRejected"] = "The event was rejected and deleted.";
            }

            return RedirectToAction(nameof(ApproveEvents));
        }

    }
}
