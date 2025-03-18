namespace MotorcycleForum.Web.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? CreatorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsOwner { get; set; }
    }
}
