using Microsoft.AspNetCore.Mvc.Rendering;
using MotorcycleForum.Data.Entities.Marketplace;


namespace MotorcycleForum.Services.Models.Marketplace
{
    public class MarketplaceFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SortOption { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }


        public IEnumerable<SelectListItem>? Categories { get; set; }
        public List<MarketplaceListing>? Listings { get; set; }
    }

}
