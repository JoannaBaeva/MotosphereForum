namespace MotorcycleForum.Services.Models.Admin
{
    public class UserBanViewModel
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public bool IsBanned { get; set; }
    }

}
