using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Entities.Marketplace;

namespace MotorcycleForum.Web.Models.Forum
{
    public class ForumPostViewModel
    {
        public Guid ForumPostId { get; set; }
        public string Title { get; set; } = null!;
        public Guid? CreatorId { get; set; }
        public string CreatorName { get; set; } = null!;
        public ForumTopic Topic { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
