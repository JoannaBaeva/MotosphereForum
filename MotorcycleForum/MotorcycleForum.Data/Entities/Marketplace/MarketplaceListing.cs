using Humanizer.Localisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Marketplace
{
    public class MarketplaceListing
    {
        [Key]
        public Guid ListingId { get; init; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid SellerId { get; init; }

        [ForeignKey(nameof(SellerId))]
        public User Seller { get; init; } = null!;

        public Guid CategoryId { get; init; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; init; } = null!;

    }
}
