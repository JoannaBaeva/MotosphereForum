using Microsoft.AspNetCore.Mvc;

namespace MotorcycleForum.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
