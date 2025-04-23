using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Services.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Services.Events
{
    public interface IEventsService
    {
        Task<EventsIndexViewModel> GetEventsIndexViewModelAsync();

        Task<EventFormViewModel> GetCreateViewModelAsync();

        Task<bool> CreateEventAsync(EventFormViewModel model, ClaimsPrincipal user);

        Task<Event?> GetEventDetailsAsync(Guid id);

        Task<bool> ToggleParticipationAsync(Guid id, ClaimsPrincipal user);

        Task<Event?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user);

        Task<bool> DeleteEventAsync(Guid id, ClaimsPrincipal user);

        Task<EventFormViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user);

        Task<bool> UpdateEventAsync(Guid id, EventFormViewModel model, ClaimsPrincipal user);
    }
}
