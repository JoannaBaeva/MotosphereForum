using MotorcycleForum.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class ForumVote
    {
        [Key]
        public int VoteId { get; set; }
        public Guid? UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }

        [Required]
        [ForeignKey(nameof(PostId))]
        public ForumPost Post { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; } = null!;

        [Required]
        public VoteType VoteType { get; set; }
    }
}
