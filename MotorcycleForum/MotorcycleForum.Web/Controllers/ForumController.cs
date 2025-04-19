using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Enums;
using MotorcycleForum.Web.Models.Forum;
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

        public async Task<IActionResult> Index(int? topicId, string search)
        {
            // Fetch topics for dropdown
            ViewBag.Topics = await _context.ForumTopics
                .OrderBy(t => t.Title)
                .Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.TopicId.ToString(),
                    Selected = t.TopicId == topicId
                })
                .ToListAsync();

            // Base query for posts
            var postsQuery = _context.ForumPosts
                .Include(p => p.Author)
                .Include(p => p.Topic)
                .AsQueryable();

            // Apply topic filter if selected
            if (topicId.HasValue)
                postsQuery = postsQuery.Where(p => p.TopicId == topicId.Value);

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
                postsQuery = postsQuery.Where(p => p.Title.Contains(search));

            // Fetch and map to ViewModel
            var posts = await postsQuery
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new ForumPostViewModel
                {
                    ForumPostId = p.ForumPostId,
                    Title = p.Title,
                    CreatorName = p.Author.FullName ?? "Unknown",
                    CreatedDate = p.CreatedDate,
                    Topic = p.Topic,
                    Upvotes = p.Upvotes,
                    Downvotes = p.Downvotes
                })
                .ToListAsync();

            // Preserve selected filter values
            ViewBag.SelectedTopicId = topicId;
            ViewBag.SearchTerm = search;

            return View(posts);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new ForumPostCreateViewModel
            {
                Topics = new SelectList(_context.ForumTopics.OrderBy(t => t.Title), "TopicId", "Title")
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumPostCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Topics = new SelectList(_context.ForumTopics, "TopicId", "Title", model.TopicId);
                return View(model);
            }

            var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var postId = Guid.NewGuid();

            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = model.Title,
                Content = model.Content,
                AuthorId = authorId,
                TopicId = model.TopicId,
                CreatedDate = DateTime.UtcNow,
                Images = new List<ForumPostImage>()
            };


            if (model.ImageFiles is not null && model.ImageFiles.Any())
            {
                var s3 = new S3Service();

                foreach (var formFile in model.ImageFiles.Take(10)) // max 10
                {
                    if (formFile.Length > 0)
                    {
                        var fileExt = Path.GetExtension(formFile.FileName);
                        var s3FileName = $"forum/{postId}/{Guid.NewGuid()}{fileExt}";

                        using var stream = formFile.OpenReadStream();
                        var imageUrl = await s3.UploadFileAsync(stream, s3FileName);

                        post.Images.Add(new ForumPostImage
                        {
                            ImageId = Guid.NewGuid(),
                            ForumPostId = postId,
                            ImageUrl = imageUrl
                        });
                    }
                }
            }

            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = postId });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return NotFound();

            if (post.AuthorId != userId)
                return Forbid();

            var model = new ForumPostEditViewModel
            {
                PostId = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                TopicId = post.TopicId,
                ExistingImageUrls = post.Images.Select(i => i.ImageUrl).ToList(),
                Topics = new SelectList(await _context.ForumTopics.OrderBy(t => t.Title).ToListAsync(), "TopicId", "Title", post.TopicId)
            };

            return View(model);
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumPostEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Topics = new SelectList(await _context.ForumTopics.OrderBy(t => t.Title).ToListAsync(), "TopicId", "Title", model.TopicId);
                return View(model);
            }

            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.AuthorId != userId)
                return Forbid();

            post.Title = model.Title;
            post.Content = model.Content;
            post.TopicId = model.TopicId;

            // Upload new images if any
            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                var totalImages = post.Images.Count + model.ImageFiles.Count;
                if (totalImages > 10)
                {
                    ModelState.AddModelError("", "You can have a maximum of 10 images total.");
                    model.Topics = new SelectList(await _context.ForumTopics.OrderBy(t => t.Title).ToListAsync(), "TopicId", "Title", model.TopicId);
                    return View(model);
                }

                var s3 = new S3Service();

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var key = $"forum/{post.ForumPostId}/{Guid.NewGuid()}{ext}";
                        using var stream = file.OpenReadStream();
                        var url = await s3.UploadFileAsync(stream, key);

                        await _context.ForumPostImages.AddAsync(new ForumPostImage
                        {
                            ImageId = Guid.NewGuid(),
                            ForumPostId = post.ForumPostId,
                            ImageUrl = url
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = post.ForumPostId });
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Topic)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.AuthorId != userId && !User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                return Forbid();

            var viewModel = new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                Topic = post.Topic
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.AuthorId != userId && !User.IsInRole("Moderator") && !User.IsInRole("Admin"))
                return Forbid();

            var s3 = new S3Service();

            foreach (var image in post.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');

                    await s3.DeleteFileAsync(key);
                }
            }

            foreach (var comment in post.Comments)
            {
                if (comment.Replies != null && comment.Replies.Any())
                {
                    _context.Comments.RemoveRange(comment.Replies);
                }
            }

            // 3. Delete the top-level comments
            _context.Comments.RemoveRange(post.Comments);

            // 4. Delete images from database
            _context.ForumPostImages.RemoveRange(post.Images);

            // 5. Delete the forum post itself
            _context.ForumPosts.Remove(post);

            await _context.SaveChangesAsync();

            TempData["ForumDeleteSuccess"] = "The post and all related content were successfully deleted.";
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Author)
                .Include(p => p.Topic)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.Author)
                .Include(p => p.Images)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userId = Guid.TryParse(userIdString, out var parsedId) ? parsedId : null;
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CreatorName = post.Author.FullName,
                CreatorProfilePictureUrl = post.Author.ProfilePictureUrl,
                Topic = post.Topic,
                Upvotes = post.Upvotes,
                Downvotes = post.Downvotes,
                HasVoted = post.Votes.Any(v => v.UserId == userId),
                UserVoteType = post.Votes.FirstOrDefault(v => v.UserId == userId)?.VoteType,
                IsOwner = userId.HasValue && post.AuthorId == userId.Value,
                ImageUrls = post.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>(),

                Comments = post.Comments
                    .Where(c => c.ParentCommentId == null)
                    .Select(c => new CommentViewModel
                    {
                        Id = c.CommentId,
                        Content = c.Content,
                        CreatedDate = c.CreatedDate,
                        CreatorName = c.Author.FullName,
                        CreatorProfilePictureUrl = c.Author.ProfilePictureUrl,
                        IsOwner = c.AuthorId == userId,
                        Replies = c.Replies.Select(r => new CommentViewModel
                        {
                            Id = r.CommentId,
                            Content = r.Content,
                            CreatedDate = r.CreatedDate,
                            CreatorName = r.Author.FullName,
                            CreatorProfilePictureUrl = r.Author.ProfilePictureUrl,
                            IsOwner = userId.HasValue && r.AuthorId == userId.Value, 
                        }).ToList()
                    }).ToList()
            };

            ViewBag.CurrentUserAvatar = currentUser?.ProfilePictureUrl ?? "/assets/img/no-image-found.png";

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
                return Json(new { success = false, message = "Invalid request." });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "You must be logged in to delete a comment." });

            var comment = await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.CommentId == request.CommentId);

            if (comment == null)
                return Json(new { success = false, message = "Comment not found." });

            if (comment.AuthorId != user.Id)
                return Json(new { success = false, message = "You can only delete your own comments." });

            // Delete replies first
            if (comment.Replies.Any())
                _context.Comments.RemoveRange(comment.Replies);

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
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToComment([FromBody] ReplyCommentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return Json(new { success = false, message = "Reply cannot be empty!" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "You must be logged in to reply." });
            }

            var parentComment = await _context.Comments.FindAsync(request.ParentCommentId);
            if (parentComment == null)
            {
                return Json(new { success = false, message = "Parent comment not found." });
            }

            var reply = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = request.Content,
                AuthorId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ForumPostId = parentComment.ForumPostId,
                ParentCommentId = request.ParentCommentId
            };

            _context.Comments.Add(reply);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Reply added!", replyId = reply.CommentId });
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
    public class ReplyCommentRequest
    {
        public Guid ParentCommentId { get; set; }
        public string Content { get; set; } = null!;
    }



