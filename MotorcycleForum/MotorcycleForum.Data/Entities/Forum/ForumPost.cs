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
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public Guid? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public User Author { get; set; } = null!;

        // Navigation Properties
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
