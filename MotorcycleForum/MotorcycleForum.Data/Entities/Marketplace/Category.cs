using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleForum.Data.Entities.Marketplace
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; init; }
        public string? Name { get; set; }

        public ICollection<MarketplaceListing> MarketplaceListings { get; set; } = new List<MarketplaceListing>();
    }

}
