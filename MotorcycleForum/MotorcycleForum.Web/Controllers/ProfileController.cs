using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Services.Models.Profile;
using MotorcycleForum.Services.Profile;

namespace MotorcycleForum.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("/Profile/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await _profileService.GetProfileDetailsAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}