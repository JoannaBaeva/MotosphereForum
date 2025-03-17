namespace MotorcycleForum.Web.Models
{
    public class ForumPostViewModel
    {
        public Guid ForumPostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
