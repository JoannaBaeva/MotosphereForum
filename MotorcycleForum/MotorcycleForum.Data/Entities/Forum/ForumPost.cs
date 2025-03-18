using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class ForumPost
    {
        [Key]
        public Guid ForumPostId { get; init; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? AuthorId { get; set; }
        public int? TopicId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; } = null!;

        [ForeignKey("TopicId")]
        public ForumTopic Topic { get; set; } = null!;
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
