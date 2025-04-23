using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Services.Models.Profile;

namespace MotorcycleForum.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly MotorcycleForumDbContext _context;

        public ProfileService(MotorcycleForumDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileViewModel?> GetProfileDetailsAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.ForumPosts)
                .Include(u => u.MarketplaceListings)
                .ThenInclude(l => l.Images)
                .Include(u => u.OrganizedEvents)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new ProfileViewModel
            {
                Username = user.FullName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                RegistrationDate = user.RegistrationDate,
                ForumPosts = user.ForumPosts.OrderByDescending(p => p.CreatedDate).ToList(),
                MarketplaceListings = user.MarketplaceListings.OrderByDescending(l => l.CreatedDate).ToList(),
                Events = user.OrganizedEvents.OrderByDescending(e => e.EventDate).ToList()
            };
        }
    }

}
