using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Forum;

namespace MotorcycleForum.Web.Models.Forum
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public bool IsOwner { get; set; }
        public string CreatorProfilePictureUrl { get; set; } = null!;
        public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
    }
}
