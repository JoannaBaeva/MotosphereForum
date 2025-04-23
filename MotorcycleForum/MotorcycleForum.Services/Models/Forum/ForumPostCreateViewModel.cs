using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MotorcycleForum.Services.Models.Forum
{
    public class ForumPostCreateViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(1)]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Please select a category.")]
        public int? TopicId { get; set; }
        public IFormFileCollection? ImageFiles { get; set; }
        public SelectList? Topics { get; set; }
    }   

}
