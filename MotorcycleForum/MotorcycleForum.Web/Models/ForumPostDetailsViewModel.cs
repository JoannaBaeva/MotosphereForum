namespace MotorcycleForum.Web.Models
{
    public class ForumPostDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatorName { get; set; } = null!;
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }



    }
}
