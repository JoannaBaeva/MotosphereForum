using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleForum.Web.Models.Events
{
    public class EventFormViewModel
    {
        public Guid EventId { get; set; }
        
        [Required(ErrorMessage = "The Title field is required.")]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(2000)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "The Location field is required.")]
        [StringLength(255)]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "The Event Date & Time field is required.")]
        [Display(Name = "Event Date & Time")]
        public DateTime? EventDate { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }

        [BindNever]
        [ValidateNever]
        public SelectList Categories { get; set; } = null!;
    }
}
