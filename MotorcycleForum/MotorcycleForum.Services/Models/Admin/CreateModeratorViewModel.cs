using System.ComponentModel.DataAnnotations;

namespace MotorcycleForum.Services.Models.Admin
{
    public class CreateModeratorViewModel
    {
        public List<UserListItem> Users { get; set; } = new();
    }

    public class UserListItem
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool IsModerator { get; set; }
    }
}
