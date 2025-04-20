using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Event_Tracker
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = null!;

        public DateTime? EventDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Location { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; } = false;

        public Guid OrganizerId { get; set; }

        [ForeignKey(nameof(OrganizerId))]
        public User Organizer { get; set; } = null!;

        [Required]
        public Guid? CategoryId { get; set; }
        public EventCategory Category { get; set; } = null!;

        public ICollection<EventParticipant> Participants { get; set; } = new List<EventParticipant>();
    }
}