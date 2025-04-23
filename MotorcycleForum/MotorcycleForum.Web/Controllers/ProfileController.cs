using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Services.Models.Profile;

namespace MotorcycleForum.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MotorcycleForumDbContext _context;

        public ProfileController(MotorcycleForumDbContext context)
        {
            _context = context;
        }

        [HttpGet("/Profile/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.ForumPosts)
                .Include(u => u.MarketplaceListings)
                .ThenInclude(l => l.Images)
                .Include(u => u.OrganizedEvents)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.FullName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                RegistrationDate = user.RegistrationDate,
                ForumPosts = user.ForumPosts.OrderByDescending(p => p.CreatedDate).ToList(),
                MarketplaceListings = user.MarketplaceListings.OrderByDescending(l => l.CreatedDate).ToList(),
                Events = user.OrganizedEvents.OrderByDescending(e => e.EventDate).ToList()
            };

            return View(model);
        }

    }
}
