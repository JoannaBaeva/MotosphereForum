using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Data.Enums;
using MotorcycleForum.Services.Forum;
using MotorcycleForum.Services.Models.Forum;
using MotorcycleForum.Services.Models.Requests;
using System.Security.Claims;

namespace MotorcycleForum.Services
{
    public class ForumService : IForumService
    {
        private readonly MotorcycleForumDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IS3Service _s3Service;

        public ForumService(MotorcycleForumDbContext context, UserManager<User> userManager, IS3Service s3Service)
        {
            _context = context;
            _userManager = userManager;
            _s3Service = s3Service;
        }

        public async Task<SelectList> GetTopicSelectListAsync(int? selectedTopicId)
        {
            var topics = await _context.ForumTopics
                .OrderBy(t => t.Title)
                .ToListAsync();

            return new SelectList(topics, "TopicId", "Title", selectedTopicId);
        }
        public async Task<List<ForumPostViewModel>> GetForumPostsAsync(int? topicId, string search)
        {
            var postsQuery = _context.ForumPosts
                .Include(p => p.Author)
                .Include(p => p.Topic)
                .AsQueryable();

            if (topicId.HasValue)
                postsQuery = postsQuery.Where(p => p.TopicId == topicId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                postsQuery = postsQuery.Where(p =>
                    p.Title.Contains(search) ||
                    (p.Author != null && p.Author.FullName.Contains(search)));
            }

            return await postsQuery
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new ForumPostViewModel
                {
                    ForumPostId = p.ForumPostId,
                    Title = p.Title,
                    CreatorName = p.Author.FullName ?? "Unknown",
                    CreatorId = p.AuthorId,
                    CreatedDate = p.CreatedDate,
                    Topic = p.Topic,
                    Upvotes = p.Upvotes,
                    Downvotes = p.Downvotes
                })
                .ToListAsync();
        }
        public async Task<ForumPostCreateViewModel> GetCreateViewModelAsync()
        {
            var topics = await _context.ForumTopics
                .OrderBy(t => t.Title)
                .ToListAsync();

            return new ForumPostCreateViewModel
            {
                Topics = new SelectList(topics, "TopicId", "Title")
            };
        }
        public async Task<Guid?> CreatePostAsync(ForumPostCreateViewModel model, ClaimsPrincipal user)
        {
            if (model == null || user == null)
                return null;

            var authorId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
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

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var formFile in model.ImageFiles.Take(10))
                {
                    if (formFile.Length > 0)
                    {
                        var fileExt = Path.GetExtension(formFile.FileName);
                        var s3FileName = $"forum/{postId}/{Guid.NewGuid()}{fileExt}";

                        using var stream = formFile.OpenReadStream();
                        var imageUrl = await _s3Service.UploadFileAsync(stream, s3FileName);

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

            return postId;
        }
        public async Task<ForumPostEditViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null || post.AuthorId != userId)
                return null;

            var topics = await _context.ForumTopics
                .OrderBy(t => t.Title)
                .ToListAsync();

            return new ForumPostEditViewModel
            {
                PostId = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                TopicId = post.TopicId,
                ExistingImageUrls = post.Images.Select(i => i.ImageUrl).ToList(),
                Topics = new SelectList(topics, "TopicId", "Title", post.TopicId)
            };
        }
        public async Task<bool> UpdatePostAsync(Guid id, ForumPostEditViewModel model, ClaimsPrincipal user)
        {
            if (model == null || user == null)
                return false;

            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null || post.AuthorId != userId)
                return false;

            post.Title = model.Title;
            post.Content = model.Content;
            post.TopicId = model.TopicId;

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                var totalImages = post.Images.Count + model.ImageFiles.Count;
                if (totalImages > 10)
                    return false;

                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var key = $"forum/{post.ForumPostId}/{Guid.NewGuid()}{ext}";
                        using var stream = file.OpenReadStream();
                        var url = await _s3Service.UploadFileAsync(stream, key);

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
            return true;
        }
        public async Task<ForumPostDetailsViewModel?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Topic)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return null;

            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.AuthorId != userId && !user.IsInRole("Moderator") && !user.IsInRole("Admin"))
                return null;

            return new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                Topic = post.Topic
            };
        }
        public async Task<bool> DeletePostAsync(Guid id, ClaimsPrincipal user)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Replies)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return false;

            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            if (post.AuthorId != userId && !user.IsInRole("Moderator") && !user.IsInRole("Admin"))
                return false;

            foreach (var image in post.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    var uri = new Uri(image.ImageUrl);
                    var key = uri.AbsolutePath.TrimStart('/');
                    await _s3Service.DeleteFileAsync(key);
                }
            }

            foreach (var comment in post.Comments)
            {
                if (comment.Replies.Any())
                    _context.Comments.RemoveRange(comment.Replies);
            }
            _context.Comments.RemoveRange(post.Comments);

            _context.ForumPostImages.RemoveRange(post.Images);
            _context.ForumPosts.Remove(post);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ForumPostDetailsViewModel?> GetDetailsViewModelAsync(Guid id, ClaimsPrincipal user)
        {
            var post = await _context.ForumPosts
                .Include(p => p.Author)
                .Include(p => p.Topic)
                .Include(p => p.Comments).ThenInclude(c => c.Author)
                .Include(p => p.Comments).ThenInclude(c => c.Replies).ThenInclude(r => r.Author)
                .Include(p => p.Images)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return null;

            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? userId = Guid.TryParse(userIdString, out var parsedId) ? parsedId : null;

            var viewModel = new ForumPostDetailsViewModel
            {
                Id = post.ForumPostId,
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                CreatorName = post.Author.FullName,
                CreatorId = post.AuthorId,
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
                        CreatorId = c.AuthorId,
                        CreatorProfilePictureUrl = c.Author.ProfilePictureUrl,
                        IsOwner = userId.HasValue && c.AuthorId == userId.Value,
                        Replies = c.Replies.Select(r => new CommentViewModel
                        {
                            Id = r.CommentId,
                            Content = r.Content,
                            CreatedDate = r.CreatedDate,
                            CreatorName = r.Author.FullName,
                            CreatorId = r.AuthorId,
                            CreatorProfilePictureUrl = r.Author.ProfilePictureUrl,
                            IsOwner = userId.HasValue && r.AuthorId == userId.Value,
                        }).ToList()
                    }).ToList()
            };

            return viewModel;
        }
        public async Task<(bool success, Guid? commentId, string message)> AddCommentAsync(AddCommentRequest request, ClaimsPrincipal user)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return (false, null, "Comment cannot be empty!");
            }

            var currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null)
            {
                return (false, null, "You must be logged in to comment.");
            }

            var post = await _context.ForumPosts.FindAsync(request.PostId);
            if (post == null)
            {
                return (false, null, "Post not found.");
            }

            var comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = request.Content,
                ForumPostId = request.PostId,
                AuthorId = currentUser.Id,
                CreatedDate = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return (true, comment.CommentId, "Comment added!");
        }
        public async Task<(bool success, string message)> DeleteCommentAsync(DeleteCommentRequest request, ClaimsPrincipal user)
        {
            if (request == null || request.CommentId == Guid.Empty)
                return (false, "Invalid request.");

            var currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null)
                return (false, "You must be logged in to delete a comment.");

            var comment = await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.CommentId == request.CommentId);

            if (comment == null)
                return (false, "Comment not found.");

            var isAdmin = user.IsInRole("Admin");
            var isMod = user.IsInRole("Moderator");
            if (comment.AuthorId != currentUser.Id && !isAdmin && !isMod)
                return (false, "You can only delete your own comments.");

            if (comment.Replies.Any())
                _context.Comments.RemoveRange(comment.Replies);

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return (true, "Comment deleted!");
        }
        public async Task<(bool success, int upvotes, int downvotes)> UpvotePostAsync(Guid id, ClaimsPrincipal user)
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return (false, 0, 0);

            var post = await _context.ForumPosts
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return (false, 0, 0);

            var existingVote = post.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == VoteType.Upvote)
                {
                    _context.Votes.Remove(existingVote);
                }
                else
                {
                    existingVote.VoteType = VoteType.Upvote;
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                post.Votes.Add(new Vote
                {
                    UserId = userId,
                    ForumPostId = id,
                    VoteType = VoteType.Upvote
                });
            }

            post.Upvotes = post.Votes.Count(v => v.VoteType == VoteType.Upvote);
            post.Downvotes = post.Votes.Count(v => v.VoteType == VoteType.Downvote);

            await _context.SaveChangesAsync();

            return (true, post.Upvotes, post.Downvotes);
        }
        public async Task<(bool success, int upvotes, int downvotes)> DownvotePostAsync(Guid id, ClaimsPrincipal user)
        {
            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return (false, 0, 0);

            var post = await _context.ForumPosts
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.ForumPostId == id);

            if (post == null)
                return (false, 0, 0);

            var existingVote = post.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.VoteType == VoteType.Downvote)
                {
                    _context.Votes.Remove(existingVote);
                }
                else
                {
                    existingVote.VoteType = VoteType.Downvote;
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                post.Votes.Add(new Vote
                {
                    UserId = userId,
                    ForumPostId = id,
                    VoteType = VoteType.Downvote
                });
            }

            post.Upvotes = post.Votes.Count(v => v.VoteType == VoteType.Upvote);
            post.Downvotes = post.Votes.Count(v => v.VoteType == VoteType.Downvote);

            await _context.SaveChangesAsync();

            return (true, post.Upvotes, post.Downvotes);
        }
        public async Task<(bool success, Guid? replyId, string message)> ReplyToCommentAsync(ReplyCommentRequest request, ClaimsPrincipal user)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return (false, null, "Reply cannot be empty!");
            }

            var userEntity = await _userManager.GetUserAsync(user);
            if (userEntity == null)
            {
                return (false, null, "You must be logged in to reply.");
            }

            var parentComment = await _context.Comments.FindAsync(request.ParentCommentId);
            if (parentComment == null)
            {
                return (false, null, "Parent comment not found.");
            }

            var reply = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = request.Content,
                AuthorId = userEntity.Id,
                CreatedDate = DateTime.UtcNow,
                ForumPostId = parentComment.ForumPostId,
                ParentCommentId = request.ParentCommentId
            };

            _context.Comments.Add(reply);
            await _context.SaveChangesAsync();

            return (true, reply.CommentId, "Reply added!");
        }
    }
}
