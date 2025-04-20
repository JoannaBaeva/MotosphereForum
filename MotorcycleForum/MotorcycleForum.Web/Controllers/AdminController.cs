using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Web.Models.Admin;
using System.Security.Claims;

namespace MotorcycleForum.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AdminController(MotorcycleForumDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Only Admins can access
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                ForumPostsCount = await _context.ForumPosts.CountAsync(),
                MarketplaceListingsCount = await _context.MarketplaceListings.CountAsync(),
                EventsCount = await _context.Events.CountAsync()
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateModerator()
        {
            var users = await _userManager.Users
                .Where(u => !(u.UserName == "motosphere.site@gmail.com"))
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.FullName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    IsModerator = _userManager.GetRolesAsync(u).Result.Contains("Moderator")
                })
                .ToListAsync();

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
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(CreateModerator));
            }

            if (!await _roleManager.RoleExistsAsync("Moderator"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Moderator"));
            }

            await _userManager.AddToRoleAsync(user, "Moderator");

            TempData["ModeratorCreated"] = $"User {user.FullName} has been promoted to Moderator!";
            return RedirectToAction(nameof(CreateModerator));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Demote(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            await _userManager.RemoveFromRoleAsync(user, "Moderator");
            TempData["ModeratorDemoted"] = $"User {user.FullName} has been demoted.";
            return RedirectToAction(nameof(CreateModerator));
        }

        //Mods can access too

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public IActionResult CreateMarketplaceCategory()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMarketplaceCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["MarketplaceCategoryCreated"] = "Marketplace category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public IActionResult CreateEventCategory()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEventCategory(EventCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.EventCategories.Add(category);
            await _context.SaveChangesAsync();

            TempData["EventCategoryCreated"] = "Event category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public IActionResult CreateForumTopic()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForumTopic(ForumTopic topic)
        {
            if (!ModelState.IsValid)
            {
                return View(topic);
            }

            topic.CreatedById = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            _context.ForumTopics.Add(topic);
            await _context.SaveChangesAsync();

            TempData["ForumTopicCreated"] = "Forum topic created successfully!";
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> BanUsers()
        {
            var users = await _context.Users
                .Select(u => new UserBanViewModel
                {
                    UserId = u.Id,
                    Email = u.Email,
                    Username = u.FullName,
                    IsBanned = _context.BannedEmails.Any(b => b.Email == u.Email)
                })
                .OrderBy(u => u.Email)
                .ToListAsync();

            return View(users);
        }


        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(BanUsers));
            }

            // Mark the user as banned
            user.IsBanned = true;

            // Add to BannedEmails if not already there
            if (!_context.BannedEmails.Any(b => b.Email == email))
            {
                _context.BannedEmails.Add(new BannedEmail
                {
                    Id = Guid.NewGuid(),
                    Email = email
                });
            }

            await _context.SaveChangesAsync();

            TempData["UserBanned"] = $"{email} has been banned.";
            return RedirectToAction(nameof(BanUsers));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnbanUser(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(BanUsers));
            }

            // Set user to not banned
            user.IsBanned = false;

            // Remove from banned emails if exists
            var bannedEmail = await _context.BannedEmails.FirstOrDefaultAsync(b => b.Email == email);
            if (bannedEmail != null)
            {
                _context.BannedEmails.Remove(bannedEmail);
            }

            await _context.SaveChangesAsync();

            TempData["UserUnbanned"] = $"{email} has been unbanned.";
            return RedirectToAction(nameof(BanUsers));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> ApproveEvents()
        {
            var pendingEvents = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Where(e => !e.IsApproved)
                .OrderBy(e => e.CreatedDate)
                .ToListAsync();

            return View(pendingEvents);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid id)
        {
            var eventToApprove = await _context.Events.FindAsync(id);

            if (eventToApprove == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction(nameof(ApproveEvents));
            }

            eventToApprove.IsApproved = true;
            await _context.SaveChangesAsync();

            TempData["EventApproved"] = "The event was successfully approved!";
            return RedirectToAction(nameof(ApproveEvents));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            var eventToReject = await _context.Events.FindAsync(id);

            if (eventToReject == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction(nameof(ApproveEvents));
            }

            _context.Events.Remove(eventToReject);
            await _context.SaveChangesAsync();

            TempData["EventRejected"] = "The event was rejected and deleted.";
            return RedirectToAction(nameof(ApproveEvents));
        }

        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Mod()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                ForumPostsCount = await _context.ForumPosts.CountAsync(),
                MarketplaceListingsCount = await _context.MarketplaceListings.CountAsync(),
                EventsCount = await _context.Events.CountAsync()
            };

            return View(model);
        }

    }
}
