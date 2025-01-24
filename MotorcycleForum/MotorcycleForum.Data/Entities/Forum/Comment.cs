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
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public Guid? AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = null!;


        public Guid? ForumPostId { get; set; }

        [ForeignKey(nameof(ForumPostId))]
        public ForumPost ForumPost { get; set; } = null!;

    }
}
