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
        public Guid ListingId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid SellerId { get; set; }

        [ForeignKey(nameof(SellerId))]
        public User Seller { get; init; } = null!;

        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; init; } = null!;

        public bool IsActive { get; set; } = true;
        public string Location { get; set; } = null!;
        public string? SellerPhoneNumber { get; set; }
        public List<MarketplaceListingImage> Images { get; set; } = new List<MarketplaceListingImage>();

        public MarketplaceListing(Guid sellerId)
        {
            SellerId = sellerId;
        }
    }


}
