namespace MotorcycleForum.Web.Models.Admin
{
    public class CreateForumTopicViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CreateForumTopicViewModel> Topics { get; set; } = new();
    }

}
