using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities
{
    public class Event
    {
        [Key]
        public Guid EventId { get; init; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid OrganizerId { get; set; }

        [ForeignKey(nameof(OrganizerId))]
        public User Organizer { get; set; } = null!;

        public ICollection<EventParticipant>? EventParticipants { get; set; } = new List<EventParticipant>();
    }
}