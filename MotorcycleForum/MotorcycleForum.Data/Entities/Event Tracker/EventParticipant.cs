using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities
{
    public class EventParticipant
    {
        [Key]
        public Guid ParticipantId { get; init; }

        public Guid? EventId { get; set; }
        public Event Event { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }

}
