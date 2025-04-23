using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Services.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MotorcycleForum.Services.Models.Requests;

namespace MotorcycleForum.Services.Forum
{
    public interface IForumService
    {
        Task<SelectList> GetTopicSelectListAsync(int? selectedTopicId);
        Task<List<ForumPostViewModel>> GetForumPostsAsync(int? topicId, string search);
        Task<ForumPostCreateViewModel> GetCreateViewModelAsync();
        Task<Guid?> CreatePostAsync(ForumPostCreateViewModel model, ClaimsPrincipal user);
        Task<ForumPostEditViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user);
        Task<bool> UpdatePostAsync(Guid id, ForumPostEditViewModel model, ClaimsPrincipal user);
        Task<ForumPostDetailsViewModel?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user);
        Task<bool> DeletePostAsync(Guid id, ClaimsPrincipal user);
        Task<ForumPostDetailsViewModel?> GetDetailsViewModelAsync(Guid id, ClaimsPrincipal user);
        Task<(bool success, Guid? commentId, string message)> AddCommentAsync(AddCommentRequest request, ClaimsPrincipal user);
        Task<(bool success, string message)> DeleteCommentAsync(DeleteCommentRequest request, ClaimsPrincipal user);
        Task<(bool success, int upvotes, int downvotes)> UpvotePostAsync(Guid id, ClaimsPrincipal user);
        Task<(bool success, int upvotes, int downvotes)> DownvotePostAsync(Guid id, ClaimsPrincipal user);
        Task<(bool success, Guid? replyId, string message)> ReplyToCommentAsync(ReplyCommentRequest request, ClaimsPrincipal user);
    }
}
