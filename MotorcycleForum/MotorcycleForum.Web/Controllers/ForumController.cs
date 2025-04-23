using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Enums;
using MotorcycleForum.Services;
using MotorcycleForum.Services.Forum;
using MotorcycleForum.Services.Models.Forum;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MotorcycleForum.Services.Models.Requests;

namespace MotorcycleForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly UserManager<User> _userManager;
        IForumService _forumService;

        public ForumController(MotorcycleForumDbContext context, UserManager<User> userManager, IForumService forumService)
        {
            _context = context;
            _userManager = userManager;
            _forumService = forumService;
        }

        public async Task<IActionResult> Index(int? topicId, string search)
        {
            ViewBag.Topics = await _forumService.GetTopicSelectListAsync(topicId);
            var posts = await _forumService.GetForumPostsAsync(topicId, search);

            ViewBag.SelectedTopicId = topicId;
            ViewBag.SearchTerm = search;

            return View(posts);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _forumService.GetCreateViewModelAsync();
            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumPostCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Topics = await _forumService.GetTopicSelectListAsync(model.TopicId);
                return View(model);
            }

            var postId = await _forumService.CreatePostAsync(model, User);

            if (postId == null)
            {
                ModelState.AddModelError("", "Failed to create post.");
                model.Topics = await _forumService.GetTopicSelectListAsync(model.TopicId);
                return View(model);
            }

            return RedirectToAction("Details", new { id = postId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _forumService.GetEditViewModelAsync(id, User);
            if (model == null)
                return Forbid();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumPostEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Topics = await _forumService.GetTopicSelectListAsync(model.TopicId);
                return View(model);
            }

            var success = await _forumService.UpdatePostAsync(id, model, User);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to update post.");
                model.Topics = await _forumService.GetTopicSelectListAsync(model.TopicId);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var viewModel = await _forumService.GetDeleteConfirmationAsync(id, User);
            if (viewModel == null)
                return Forbid();

            return View(viewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _forumService.DeletePostAsync(id, User);

            if (!success)
            {
                TempData["ForumDeleteFailed"] = "Something went wrong while deleting the post.";
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["ForumDeleteSuccess"] = "The post and all related content were successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var viewModel = await _forumService.GetDetailsViewModelAsync(id, User);

            if (viewModel == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserAvatar = currentUser?.ProfilePictureUrl ?? "/assets/img/no-image-found.png";

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
        {
            var result = await _forumService.AddCommentAsync(request, User);

            return Json(new
            {
                success = result.success,
                message = result.message,
                commentId = result.commentId
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request)
        {
            var result = await _forumService.DeleteCommentAsync(request, User);

            return Json(new
            {
                success = result.success,
                message = result.message
            });
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upvote(Guid id)
        {
            var result = await _forumService.UpvotePostAsync(id, User);

            return Json(new
            {
                success = result.success,
                upvotes = result.upvotes,
                downvotes = result.downvotes
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Downvote(Guid id)
        {
            var result = await _forumService.DownvotePostAsync(id, User);

            return Json(new
            {
                success = result.success,
                upvotes = result.upvotes,
                downvotes = result.downvotes
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToComment([FromBody] ReplyCommentRequest request)
        {
            var result = await _forumService.ReplyToCommentAsync(request, User);

            return Json(new
            {
                success = result.success,
                message = result.message,
                replyId = result.replyId
            });
        }

    }
}






