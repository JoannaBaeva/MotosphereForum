namespace MotorcycleForum.Services.Models.Admin
{
    public class CreateEventCategoryViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<CreateEventCategoryViewModel> Categories { get; set; } = new();
    }

}
