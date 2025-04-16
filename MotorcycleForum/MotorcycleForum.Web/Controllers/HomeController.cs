using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Web.Models;
using System.Diagnostics;

namespace MotorcycleForum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Privacy() => View();

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
                return View("Error400");

            return View();
        }


    }
}
