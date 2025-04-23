using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MotorcycleForum.Services.Models.Forum
{
    public class ForumPostEditViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(1)]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Please select a topic.")]
        public int? TopicId { get; set; }

        public List<IFormFile> ImageFiles { get; set; } = new();

        public List<string> ExistingImageUrls { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public SelectList Topics { get; set; } = null!;

        public Guid PostId { get; set; }
    }
}
