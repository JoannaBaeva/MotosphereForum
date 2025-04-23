namespace MotorcycleForum.Services.Models.Admin
{
    public class CreateMarketplaceCategoryViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<CreateMarketplaceCategoryViewModel> Categories { get; set; } = new();
    }
}
