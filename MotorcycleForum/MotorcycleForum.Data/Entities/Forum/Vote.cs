using MotorcycleForum.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Forum
{
    public class Vote
    {
        [Key]
        public Guid VoteId { get; set; }

        public Guid UserId { get; set; }
        public Guid ForumPostId { get; set; }

        public VoteType VoteType { get; set; } 

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ForumPostId")]
        public ForumPost? ForumPost { get; set; }
    }
}
