using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Enums;

namespace MotorcycleForum.Services.Models.Forum
{
    public class ForumPostDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatorName { get; set; } = null!;
        public string CreatorProfilePictureUrl { get; set; } = null!;
        public Guid? CreatorId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public ForumTopic Topic { get; set; } = null!;
        public bool HasVoted { get; set; }
        public bool IsOwner { get; set; }
        public VoteType? UserVoteType { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<string> ImageUrls { get; set; } = new();
    }
}
