using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Services.Models;
using MotorcycleForum.Services.Models.Home;
using System.Diagnostics;
using MotorcycleForum.Data;

namespace MotorcycleForum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MotorcycleForumDbContext _context;

        public HomeController(ILogger<HomeController> logger, MotorcycleForumDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.UtcNow;

            var viewModel = new HomePageViewModel
            {
                UpcomingEvents = await _context.Events
                    .Where(e => e.IsApproved && e.EventDate > now)
                    .OrderBy(e => e.EventDate)
                    .Take(3)
                    .Include(e => e.Category)
                    .ToListAsync(),

                LatestListings = await _context.MarketplaceListings
                    .Where(l => l.IsActive)
                    .OrderByDescending(l => l.CreatedDate)
                    .Take(3)
                    .Include(l => l.Category)
                    .Include(l => l.Images)
                    .ToListAsync(),

                TrendingPosts = await _context.ForumPosts
                    .OrderByDescending(p => p.Votes.Count)
                    .Take(3)
                    .Include(p => p.Topic)
                    .ToListAsync()
            };

            return View(viewModel);
        }
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
