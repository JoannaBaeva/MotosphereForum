using MotorcycleForum.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Roles
{
    public class Moderator
    {
        [Key]
        public Guid ModeratorId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

    }
    public class TopicApprovalRequest
    {
        public int TopicId { get; set; }
        public Guid ModeratorId { get; set; }
    }
}
