using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Services.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorcycleForum.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Data.Entities.Event_Tracker;
using System.Security.Claims;

namespace MotorcycleForum.Services.Events
{
    public class EventsService : IEventsService
    {
        private readonly MotorcycleForumDbContext _context;
        
        public EventsService(MotorcycleForumDbContext context)
        {
            _context = context;
        }
        
        public async Task<EventsIndexViewModel> GetEventsIndexViewModelAsync()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Participants)
                .Where(e => e.IsApproved)
            .OrderBy(e => e.EventDate)
                .ToListAsync();

            var categories = await _context.EventCategories.ToListAsync();

            return new EventsIndexViewModel
            {
                Events = events,
                Categories = categories
            };
        }

        public async Task<EventFormViewModel> GetCreateViewModelAsync()
        {
            var categories = await _context.EventCategories.ToListAsync();

            return new EventFormViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "Name")
            };
        }
        
        public async Task<bool> CreateEventAsync(EventFormViewModel model, ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return false;

            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var newEvent = new Event
            {
                EventId = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location,
                CategoryId = model.CategoryId,
                OrganizerId = userId,
                IsApproved = false
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Event?> GetEventDetailsAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Participants)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(e => e.EventId == id);
        }

        public async Task<bool> ToggleParticipationAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventId == id && e.IsApproved);

            if (eventItem == null)
                return false;

            var existingParticipant = eventItem.Participants.FirstOrDefault(p => p.UserId == userId);

            if (existingParticipant != null)
            {
                _context.EventParticipants.Remove(existingParticipant);
            }
            else
            {
                var newParticipant = new EventParticipant
                {
                    EventParticipantId = Guid.NewGuid(),
                    EventId = id,
                    UserId = userId
                };
                await _context.EventParticipants.AddAsync(newParticipant);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Event?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return null;

            if (eventItem.OrganizerId != userId &&
                !user.IsInRole("Moderator") &&
                !user.IsInRole("Admin"))
                return null;

            return eventItem;
        }

        public async Task<bool> DeleteEventAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return false;

            if (eventItem.OrganizerId != userId &&
                !user.IsInRole("Moderator") &&
                !user.IsInRole("Admin"))
                return false;

            _context.EventParticipants.RemoveRange(eventItem.Participants);
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EventFormViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return null;

            if (eventItem.OrganizerId != userId &&
                !user.IsInRole("Moderator") &&
                !user.IsInRole("Admin"))
                return null;

            var viewModel = new EventFormViewModel
            {
                EventId = eventItem.EventId,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                EventDate = eventItem.EventDate,
                CategoryId = eventItem.CategoryId,
                Categories = new SelectList(await _context.EventCategories.ToListAsync(), "CategoryId", "Name", eventItem.CategoryId)
            };

            return viewModel;
        }

        public async Task<bool> UpdateEventAsync(Guid id, EventFormViewModel model, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return false;

            if (eventItem.OrganizerId != userId &&
                !user.IsInRole("Moderator") &&
                !user.IsInRole("Admin"))
                return false;

            eventItem.Title = model.Title;
            eventItem.Description = model.Description;
            eventItem.Location = model.Location;
            eventItem.EventDate = model.EventDate;
            eventItem.CategoryId = model.CategoryId;

            if (!user.IsInRole("Moderator") && !user.IsInRole("Admin"))
                eventItem.IsApproved = false;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
