using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;

namespace MotorcycleForum.Services.Models.Home
{
    public class HomePageViewModel
    {
        public List<Event> UpcomingEvents { get; set; } = new List<Event>();
        public List<MarketplaceListing> LatestListings { get; set; } = new List<MarketplaceListing>();
        public List<ForumPost> TrendingPosts { get; set; } = new List<ForumPost>();
    }

}
