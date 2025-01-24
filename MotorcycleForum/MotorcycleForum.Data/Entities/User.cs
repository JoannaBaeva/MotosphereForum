using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public ICollection<Event>? OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<MarketplaceListing> MarketplaceListings { get; set; } = new List<MarketplaceListing>();
        public ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
