using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;

namespace MotorcycleForum.Services.Models.Profile
{
    public class ProfileViewModel
    {
        public string? Username { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<ForumPost> ForumPosts { get; set; } = new();
        public List<MarketplaceListing> MarketplaceListings { get; set; } = new();
        public List<Event> Events { get; set; } = new();
    }
}
