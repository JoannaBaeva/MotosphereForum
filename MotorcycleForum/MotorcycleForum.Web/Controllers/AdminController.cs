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
using MotorcycleForum.Web.Models.Admin;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //Create Moderator
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

        // Mod Panel
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
        
        // Create Marketplace Category

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateMarketplaceCategory()
        {
            var categories = await _context.Categories
                .Select(u => new CreateMarketplaceCategoryViewModel()
                {
                    Id = u.CategoryId,
                    Name = u.Name
                })
                .ToListAsync();
            
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
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Category name is required.");
                return View(category);
            }

            var newCategory = new Category { CategoryId = Guid.NewGuid(), Name = category.Name };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["MarketplaceCategoryCreated"] = "Marketplace category created successfully!";
            return RedirectToAction(nameof(CreateMarketplaceCategory));
        }
        
        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMarketplaceCategory(Guid id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }
            
            var listingsInCategory = await _context.MarketplaceListings
                .Where(l => l.CategoryId == id)
                .ToListAsync();

            foreach (var listing in listingsInCategory)
            {
                await DeleteListingDataAsync(listing.ListingId);
            }
            
            _context.Categories.Remove(categoryToDelete);
            
            await _context.SaveChangesAsync();
            
            TempData["MarketplaceCategoryDeleted"] = "Marketplace category deleted successfully!";
            return RedirectToAction(nameof(CreateMarketplaceCategory));
        }
        
        private async Task DeleteListingDataAsync(Guid listingId)
        {
            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == listingId);
            if (listing == null)
                return;
            var s3 = new S3Service();
            foreach (var image in listing.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');
                    await s3.DeleteFileAsync(key);
                }
            }
            _context.MarketplaceListingImages.RemoveRange(listing.Images);
            _context.MarketplaceListings.Remove(listing);
        }

        // Create Event Category

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateEventCategory()
        {
            var categories = await _context.EventCategories
                .Select(u => new CreateEventCategoryViewModel()
                {
                    Id = u.CategoryId,
                    Name = u.Name
                })
                .ToListAsync();
            
            var model = new CreateEventCategoryViewModel { Categories = categories };
            return View(model);

        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEventCategory(CreateEventCategoryViewModel category)
        {
            if (category.Name == null)
            {
                ModelState.AddModelError("Name", "Category name is required.");
                return View(category);
            }

            var newCategory = new EventCategory { CategoryId = Guid.NewGuid(), Name = category.Name };

            _context.EventCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["EventCategoryCreated"] = "Event category created successfully!";
            return RedirectToAction(nameof(CreateEventCategory));
        }

        [Authorize(Roles = "Moderator, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventCategory(Guid id)
        {
            var categoryToDelete = await _context.EventCategories.FindAsync(id);
            if (categoryToDelete == null)
            {
                return NotFound();
            }

            _context.EventCategories.Remove(categoryToDelete);
            _context.Events.RemoveRange(_context.Events.Where(e => e.CategoryId == id));
            
            await _context.SaveChangesAsync();
            TempData["EventCategoryDeleted"] = "Event category deleted successfully!";
            return RedirectToAction(nameof(CreateEventCategory));
        }



        //Create Forum Topic
        
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet]
        public async Task<IActionResult> CreateForumTopic()
        {
            var topics = await _context.ForumTopics
                .Select(t => new CreateForumTopicViewModel()
                {
                    Id = t.TopicId,
                    Name = t.Title
                })
                .ToListAsync();

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
                return View(topic);
            }

            var newTopic = new ForumTopic { Title = topic.Name };

            _context.ForumTopics.Add(newTopic);
            await _context.SaveChangesAsync();

            TempData["ForumTopicCreated"] = "Forum topic created successfully!";
            return RedirectToAction(nameof(CreateForumTopic));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForumTopic(int id)
        {
            var topicToDelete = await _context.ForumTopics.FindAsync(id);
            if (topicToDelete == null)
            {
                return NotFound();
            }

            var postsInTopic = await _context.ForumPosts
                .Where(p => p.TopicId == id)
                .ToListAsync();

            foreach (var post in postsInTopic)
            {
                await DeletePostDataAsync(post.ForumPostId);
            }

            _context.ForumTopics.Remove(topicToDelete);
            await _context.SaveChangesAsync();

            TempData["ForumTopicDeleted"] = "Forum topic deleted successfully!";
            return RedirectToAction(nameof(CreateForumTopic));
        }

        private async Task DeletePostDataAsync(Guid postId)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(p => p.ForumPostId == postId);

            if (post == null)
                return;

            var s3 = new S3Service();

            foreach (var image in post.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');

                    await s3.DeleteFileAsync(key);
                }
            }

            foreach (var comment in post.Comments)
            {
                if (comment.Replies != null && comment.Replies.Any())
                {
                    _context.Comments.RemoveRange(comment.Replies);
                }
            }

            _context.Comments.RemoveRange(post.Comments);
            _context.ForumPostImages.RemoveRange(post.Images);
            _context.ForumPosts.Remove(post);
        }


        // Ban Users
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


        // Approve Events
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


    }
}
