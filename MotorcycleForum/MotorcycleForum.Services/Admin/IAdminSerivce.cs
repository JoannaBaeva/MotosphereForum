using MotorcycleForum.Services.Models.Admin;
using MotorcycleForum.Data.Entities.Event_Tracker;

namespace MotorcycleForum.Services.Admin
{
    public interface IAdminService
    {
        // Dashboard
        Task<AdminDashboardViewModel> GetDashboardStatsAsync();

        // Moderators
        Task<List<UserListItem>> GetModeratorCandidatesAsync();
        Task<bool> PromoteToModeratorAsync(Guid userId);
        Task<bool> DemoteFromModeratorAsync(Guid userId);

        // Marketplace Category
        Task<List<CreateMarketplaceCategoryViewModel>> GetMarketplaceCategoriesAsync();
        Task<bool> CreateMarketplaceCategoryAsync(string name);
        Task<bool> DeleteMarketplaceCategoryAsync(Guid id);

        // Event Category
        Task<List<CreateEventCategoryViewModel>> GetEventCategoriesAsync();
        Task<bool> CreateEventCategoryAsync(string name);
        Task<bool> DeleteEventCategoryAsync(Guid id);

        // Forum Topics
        Task<List<CreateForumTopicViewModel>> GetForumTopicsAsync();
        Task<bool> CreateForumTopicAsync(string title);
        Task<bool> DeleteForumTopicAsync(int id);
        Task DeletePostDataAsync(Guid postId);


        // Ban/Unban Users
        Task<List<UserBanViewModel>> GetBanUsersListAsync(string? currentUserEmail);
        Task<bool> BanUserAsync(string email);
        Task<bool> UnbanUserAsync(string email);

        // Event Approvals
        Task<List<Event>> GetPendingEventsAsync();
        Task<bool> ApproveEventAsync(Guid id);
        Task<bool> RejectEventAsync(Guid id);
    }
}