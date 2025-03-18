using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Enums;
using MotorcycleForum.Web.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MotorcycleForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly UserManager<User> _userManager;

        public ForumController(MotorcycleForumDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var posts = _context.ForumPosts?
                .AsNoTracking()
                .Select(p => new ForumPostViewModel
                {
                    ForumPostId = p.ForumPostId,
                    Title = p.Title,
                    CreatorName = p.Author.FullName ?? "Unknown",
                    CreatedDate = p.CreatedDate,
                    Upvotes = p.Upvotes,
                    Downvotes = p.Downvotes
                })
                .ToList();

            return View(posts);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var post = await _context.ForumPosts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
            {
                TempData["ErrorMessage"] = "The requested post does not exist.";
                return RedirectToAction("Index");
            }

            var viewModel = new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CreatorName = post.Author?.FullName ?? "Unknown",
                Upvotes = post.Upvotes,
                Downvotes = post.Downvotes,
                Comments = post.Comments.Select(c => new CommentViewModel
                {
                    Id = c.CommentId,
                    Content = c.Content,
                    CreatorName = c.Author?.FullName ?? "Unknown",
                    CreatedDate = c.CreatedDate,
                    IsOwner = c.AuthorId == userId
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return Json(new { success = false, message = "Comment cannot be empty!" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "You must be logged in to comment." });
            }

            var post = await _context.ForumPosts.FindAsync(request.PostId);
            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }

            var comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = request.Content,
                ForumPostId = request.PostId,
                AuthorId = user.Id,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments?.Add(comment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Comment added!", commentId = comment.CommentId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request)
        {
            if (request == null || request.CommentId == Guid.Empty)
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "You must be logged in to delete a comment." });
            }

            var comment = await _context.Comments.FindAsync(request.CommentId);
            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found." });
            }

            // Ensure the logged-in user is the owner of the comment
            if (comment.AuthorId != user.Id)
            {
                return Json(new { success = false, message = "You can only delete your own comments." });
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Comment deleted!" });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await _context.ForumPosts
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }

            var existingVote = post.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == VoteType.Upvote)
                {
                    _context.Votes.Remove(existingVote); // Remove upvote
                }
                else
                {
                    existingVote.VoteType = VoteType.Upvote; // Change to upvote
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                post.Votes.Add(new Vote { UserId = userId, ForumPostId = id, VoteType = VoteType.Upvote });
            }

            // Update post vote counts
            post.Upvotes = post.Votes.Count(v => v.VoteType == VoteType.Upvote);
            post.Downvotes = post.Votes.Count(v => v.VoteType == VoteType.Downvote);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                upvotes = post.Upvotes,
                downvotes = post.Downvotes
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Downvote(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var post = await _context.ForumPosts
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
            {
                return Json(new { success = false, message = "Post not found." });
            }

            var existingVote = post.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == VoteType.Downvote)
                {
                    _context.Votes.Remove(existingVote); // Remove downvote
                }
                else
                {
                    existingVote.VoteType = VoteType.Downvote; // Change to downvote
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                post.Votes.Add(new Vote { UserId = userId, ForumPostId = id, VoteType = VoteType.Downvote });
            }

            // Update post vote counts
            post.Upvotes = post.Votes.Count(v => v.VoteType == VoteType.Upvote);
            post.Downvotes = post.Votes.Count(v => v.VoteType == VoteType.Downvote);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                upvotes = post.Upvotes,
                downvotes = post.Downvotes
            });
        }

    }
}
    public class AddCommentRequest
    {
        public Guid PostId { get; set; }
        public string? Content { get; set; }
    } 

    public class DeleteCommentRequest
    {
        public Guid CommentId { get; set; }
    }


