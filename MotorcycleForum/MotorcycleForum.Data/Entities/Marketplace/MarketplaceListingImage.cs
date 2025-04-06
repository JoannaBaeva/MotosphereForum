using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Marketplace
{
    public class MarketplaceListingImage
    {
        [Key]
        public Guid ImageId { get; set; }

        public string ImageUrl { get; set; } = null!; // Store the S3 image URL

        public Guid ListingId { get; set; }

        [ForeignKey(nameof(ListingId))]
        public MarketplaceListing? Listing { get; init; } = null!;
    }

}
