namespace MotorcycleForum.Web.Models
{
    public class ForumTopicViewModel
    {
        public int TopicId { get; set; }
        public string Title { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
