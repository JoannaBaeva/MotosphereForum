using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MotorcycleForum.Web.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Moderator")]
        public IActionResult Mod()
        {
            return View();
        }
    }
}
