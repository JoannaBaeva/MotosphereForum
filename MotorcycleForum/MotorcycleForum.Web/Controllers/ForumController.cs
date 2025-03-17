using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var posts = _context.ForumPosts?
                .Select(p => new ForumPostViewModel
                {
                    ForumPostId = p.ForumPostId,
                    Title = p.Title,
                    CreatorName = p.Author.FullName ?? "Unknown",
                    CreatedDate = p.CreatedDate
                })
                .ToList();

            return View(posts);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
            {
                return NotFound();
            }

            // Map to ForumPostDetailsViewModel
            var viewModel = new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CreatorName = post.Author?.FullName ?? "Unknown", // Fallback if Author is null
                Upvotes = post.Upvotes, // Assuming Upvotes is a property in your ForumPost entity
                Downvotes = post.Downvotes // Assuming Downvotes is a property in your ForumPost entity
            };

            return View(viewModel);
        }
    }

}
