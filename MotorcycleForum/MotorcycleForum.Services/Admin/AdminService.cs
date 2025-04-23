using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Services.Models.Admin;


namespace MotorcycleForum.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly S3Service _s3Service;

        public AdminService(MotorcycleForumDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, S3Service s3Service)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _s3Service = s3Service;
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                ForumPostsCount = await _context.ForumPosts.CountAsync(),
                MarketplaceListingsCount = await _context.MarketplaceListings.CountAsync(),
                EventsCount = await _context.Events.CountAsync()
            };

            return model;
        }

        public async Task<List<UserListItem>> GetModeratorCandidatesAsync()
        {
            var users = await _context.Users
                .Where(u => u.UserName != "motosphere.site@gmail.com")
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.FullName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    IsModerator = _context.UserRoles
                        .Any(ur => ur.UserId == u.Id &&
                                   _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Moderator"))
                })
                .ToListAsync();

            return users;
        }
        public async Task<bool> PromoteToModeratorAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var roleExists = await _roleManager.RoleExistsAsync("Moderator");
            if (!roleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>("Moderator"));
                if (!createRoleResult.Succeeded)
                    return false;
            }

            var result = await _userManager.AddToRoleAsync(user, "Moderator");
            return result.Succeeded;
        }

        public async Task<bool> DemoteFromModeratorAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, "Moderator");
            return result.Succeeded;
        }

        public async Task<List<CreateMarketplaceCategoryViewModel>> GetMarketplaceCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CreateMarketplaceCategoryViewModel
                {
                    Id = c.CategoryId,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CreateMarketplaceCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task DeleteListingDataAsync(Guid listingId)
        {
            var listing = await _context.MarketplaceListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.ListingId == listingId);

            if (listing == null)
                return;

            foreach (var image in listing.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');
                    await _s3Service.DeleteFileAsync(key);
                }
            }

            _context.MarketplaceListingImages.RemoveRange(listing.Images);
            _context.MarketplaceListings.Remove(listing);
        }

        public async Task<bool> DeleteMarketplaceCategoryAsync(Guid id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if (categoryToDelete == null)
                return false;

            var listingsInCategory = await _context.MarketplaceListings
                .Where(l => l.CategoryId == id)
                .ToListAsync();

            foreach (var listing in listingsInCategory)
            {
                await DeleteListingDataAsync(listing.ListingId);
            }

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CreateEventCategoryViewModel>> GetEventCategoriesAsync()
        {
            return await _context.EventCategories
                .Select(c => new CreateEventCategoryViewModel
                {
                    Id = c.CategoryId,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<bool> CreateEventCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var newCategory = new EventCategory
            {
                CategoryId = Guid.NewGuid(),
                Name = name
            };

            _context.EventCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEventCategoryAsync(Guid id)
        {
            var categoryToDelete = await _context.EventCategories.FindAsync(id);
            if (categoryToDelete == null)
                return false;

            var eventsToRemove = await _context.Events
                .Include(e => e.Participants)
                .Where(e => e.CategoryId == id)
                .ToListAsync();

            foreach (var ev in eventsToRemove)
            {
                _context.EventParticipants.RemoveRange(ev.Participants);
            }

            _context.Events.RemoveRange(eventsToRemove);
            _context.EventCategories.Remove(categoryToDelete);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<CreateForumTopicViewModel>> GetForumTopicsAsync()
        {
            return await _context.ForumTopics
                .Select(t => new CreateForumTopicViewModel
                {
                    Id = t.TopicId,
                    Name = t.Title
                })
                .ToListAsync();
        }

        public async Task<bool> CreateForumTopicAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;

            var topic = new ForumTopic
            {
                Title = title
            };

            _context.ForumTopics.Add(topic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteForumTopicAsync(int id)
        {
            var topic = await _context.ForumTopics.FindAsync(id);
            if (topic == null)
                return false;

            var posts = await _context.ForumPosts
                .Where(p => p.TopicId == id)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replies)
                .ToListAsync();

            foreach (var post in posts)
            {
                foreach (var image in post.Images)
                {
                    if (!string.IsNullOrEmpty(image.ImageUrl))
                    {
                        var key = new Uri(image.ImageUrl).AbsolutePath.TrimStart('/');
                        await _s3Service.DeleteFileAsync(key);
                    }
                }

                _context.Comments.RemoveRange(post.Comments.SelectMany(c => c.Replies));
                _context.Comments.RemoveRange(post.Comments);
                _context.ForumPostImages.RemoveRange(post.Images);
            }

            _context.ForumPosts.RemoveRange(posts);
            _context.ForumTopics.Remove(topic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeletePostDataAsync(Guid postId)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(p => p.ForumPostId == postId);

            if (post == null)
                return;

            foreach (var image in post.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');
                    await _s3Service.DeleteFileAsync(key);
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

            await _context.SaveChangesAsync();
        }


        public async Task<List<UserBanViewModel>> GetBanUsersListAsync(string? currentUserEmail)
        {
            var bannedEmails = await _context.BannedEmails.Select(b => b.Email).ToListAsync();

            var users = await _context.Users
                .Where(u => u.Email != "motosphere.site@gmail.com" && u.Email != currentUserEmail)
                .Select(u => new UserBanViewModel
                {
                    UserId = u.Id,
                    Email = u.Email,
                    Username = u.FullName,
                    IsBanned = bannedEmails.Contains(u.Email)
                })
                .OrderBy(u => u.Email)
                .ToListAsync();

            return users;
        }

        public async Task<bool> BanUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            if (!await _context.BannedEmails.AnyAsync(b => b.Email == email))
            {
                _context.BannedEmails.Add(new BannedEmail
                {
                    Id = Guid.NewGuid(),
                    Email = email
                });
                
                user.IsBanned = true;

                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UnbanUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var bannedEmail = await _context.BannedEmails.FirstOrDefaultAsync(b => b.Email == email);
            if (bannedEmail != null)
            {
                _context.BannedEmails.Remove(bannedEmail);
                user.IsBanned = false;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        public async Task<List<Event>> GetPendingEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Where(e => !e.IsApproved)
                .OrderBy(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> ApproveEventAsync(Guid id)
        {
            var eventToApprove = await _context.Events.FindAsync(id);
            if (eventToApprove == null) return false;

            eventToApprove.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> RejectEventAsync(Guid id)
        {
            var eventToReject = await _context.Events.FindAsync(id);
            if (eventToReject == null) return false;

            _context.Events.Remove(eventToReject);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
