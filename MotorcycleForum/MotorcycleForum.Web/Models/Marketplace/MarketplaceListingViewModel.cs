using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Data.Entities.Marketplace;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MotorcycleForum.Web.Models.Marketplace
{


    public class MarketplaceListingViewModel
    {
        // Fields shown in the form
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = null!;

        // Images uploaded during form submission
        [Display(Name = "Upload Images")]
        public List<IFormFile> ImageFiles { get; set; } = new();

        // Display uploaded image URLs (for Edit view or confirmation)
        public List<string> ExistingImageUrls { get; set; } = new();
        public List<Guid> ImageIds { get; set; } = new();

        // Dropdown list
        [BindNever]
        [ValidateNever]
        public SelectList Categories { get; set; } = null!;

        // Used during editing, or for internal logic
        public Guid? ListingId { get; set; }
        public Guid? SellerId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
