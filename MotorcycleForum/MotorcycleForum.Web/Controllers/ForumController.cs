using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Data;
using MotorcycleForum.Web.Models;
using System;

namespace MotorcycleForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly MotorcycleForumDbContext _context;

        public ForumController(MotorcycleForumDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var topics = _context.ForumTopics
                .Where(t => t.IsApproved)
                .Select(t => new ForumTopicViewModel
                {
                    TopicId = t.TopicId,
                    Title = t.Title,
                    CreatorName = t.Creator.UserName,
                    CreatedDate = t.CreatedDate
                })
                .ToList();

            return View(topics);
        }
    }

}
