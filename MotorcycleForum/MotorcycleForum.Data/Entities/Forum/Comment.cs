using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; init; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = null!;

        public Guid ForumPostId { get; set; }

        [ForeignKey(nameof(ForumPostId))]
        public ForumPost ForumPost { get; set; } = null!;
        public Guid? ParentCommentId { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public Comment? ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
