using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MotorcycleForum.Services.Models.Marketplace
{
    public class MarketplaceListingViewModel
    {
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

        [Display(Name = "Upload Images")]
        public List<IFormFile> ImageFiles { get; set; } = new();

        public List<string> ExistingImageUrls { get; set; } = new();
        public List<Guid> ImagesToDelete { get; set; } = new();
        public List<Guid> ImageIds { get; set; } = new();

        [BindNever]
        [ValidateNever]
        public SelectList Categories { get; set; } = null!;

        public Guid? ListingId { get; set; }
        public Guid? SellerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}