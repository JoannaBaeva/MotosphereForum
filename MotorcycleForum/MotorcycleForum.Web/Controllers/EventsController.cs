using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Services.Models.Events;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Services.Events;
using Microsoft.AspNetCore.Identity;
using MotorcycleForum.Data.Entities;
namespace MotorcycleForum.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = await _eventsService.GetEventsIndexViewModelAsync();
            return View(model);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = await _eventsService.GetCreateViewModelAsync();
            return View(viewModel);
        }


        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _eventsService.GetCreateViewModelAsync().ContinueWith(t => t.Result.Categories);
                return View(model);
            }

            var success = await _eventsService.CreateEventAsync(model, User);

            if (success)
            {
                TempData["EventCreated"] = "Your event has been submitted and awaits moderator approval!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "An error occurred while creating the event.");
            model.Categories = await _eventsService.GetCreateViewModelAsync().ContinueWith(t => t.Result.Categories);
            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var eventItem = await _eventsService.GetEventDetailsAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleParticipation(Guid id)
        {
            var success = await _eventsService.ToggleParticipationAsync(id, User);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eventItem = await _eventsService.GetDeleteConfirmationAsync(id, User);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _eventsService.DeleteEventAsync(id, User);

            if (!result)
                return Forbid();

            TempData["EventDeleted"] = "Event was successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var viewModel = await _eventsService.GetEditViewModelAsync(id, User);

            if (viewModel == null)
                return Forbid();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(await _eventsService.GetCreateViewModelAsync().ContinueWith(t => t.Result.Categories), "Value", "Text", model.CategoryId);
                return View(model);
            }

            var success = await _eventsService.UpdateEventAsync(id, model, User);

            if (!success)
                return Forbid();

            TempData["EventUpdated"] = "Event updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

    }
}

