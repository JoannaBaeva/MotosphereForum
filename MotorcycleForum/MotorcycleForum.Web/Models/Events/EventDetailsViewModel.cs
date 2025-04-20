using MotorcycleForum.Data.Entities.Event_Tracker;

namespace MotorcycleForum.Web.Models.Events
{
    public class EventDetailsViewModel
    {
        public Event Event { get; set; } = null!;
        public bool IsParticipating { get; set; }
    }

}
