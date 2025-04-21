using MotorcycleForum.Data.Entities.Event_Tracker;

namespace MotorcycleForum.Web.Models.Events
{
    public class EventsIndexViewModel
    {
        public List<Event> Events { get; set; } = new();
        public List<EventCategory> Categories { get; set; } = new();
    }
}
