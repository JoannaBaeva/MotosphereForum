using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Web.Models.Events;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MotorcycleForum.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly MotorcycleForumDbContext _context;

        public EventsController(MotorcycleForumDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Participants)
                .Where(e => e.IsApproved)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }


        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EventFormViewModel
            {
                Categories = new SelectList(await _context.EventCategories.ToListAsync(), "CategoryId", "Name")
            };

            return View(viewModel);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(await _context.EventCategories.ToListAsync(), "CategoryId", "Name");
                return View(model);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var newEvent = new Event
            {
                EventId = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                EventDate = model.EventDate,
                Location = model.Location,
                CategoryId = model.CategoryId,
                OrganizerId = userId,
                IsApproved = false // Needs approval
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            TempData["EventCreated"] = "Your event has been submitted and awaits moderator approval!";
            return RedirectToAction(nameof(Index));

        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Participants)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleParticipation(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventId == id && e.IsApproved);

            if (eventItem == null)
                return NotFound();

            var existingParticipant = eventItem.Participants.FirstOrDefault(p => p.UserId == userId);

            if (existingParticipant != null)
            {
                _context.EventParticipants.Remove(existingParticipant);
            }
            else
            {
                var participant = new EventParticipant
                {
                    EventParticipantId = Guid.NewGuid(),
                    EventId = eventItem.EventId,
                    UserId = userId
                };
                await _context.EventParticipants.AddAsync(participant);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return NotFound();

            // Only the organizer, mod, or admin can delete
            if (eventItem.OrganizerId != userId && !User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                return Forbid();

            return View(eventItem);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return NotFound();

            if (eventItem.OrganizerId != userId && !User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                return Forbid();

            // Delete related participants
            _context.EventParticipants.RemoveRange(eventItem.Participants);

            // Finally delete event
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            TempData["EventDeleted"] = "Event was successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return NotFound();

            if (eventItem.OrganizerId != userId)
                return Forbid();

            var viewModel = new EventFormViewModel
            {
                EventId = eventItem.EventId,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Location = eventItem.Location,
                EventDate = eventItem.EventDate,
                CategoryId = eventItem.CategoryId,
                Categories = new SelectList(_context.EventCategories, "CategoryId", "Name", eventItem.CategoryId)
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(_context.EventCategories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return NotFound();

            if (eventItem.OrganizerId != userId && !User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                return Forbid();

            eventItem.Title = model.Title;
            eventItem.Description = model.Description;
            eventItem.Location = model.Location;
            eventItem.EventDate = model.EventDate;
            eventItem.CategoryId = model.CategoryId;

            // Reset approval after edit
            if (!User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                eventItem.IsApproved = false;

            await _context.SaveChangesAsync();

            TempData["EventUpdated"] = "Event updated successfully.";
            return RedirectToAction(nameof(Details), new { id = eventItem.EventId });
        }

    }
}

