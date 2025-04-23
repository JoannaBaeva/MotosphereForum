using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Services.Models.Marketplace;

namespace MotorcycleForum.Services.Marketplace
{
    public interface IMarketplaceService
    {
        Task<MarketplaceFilterViewModel> GetFilteredListingsAsync(
            string? searchTerm,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortOption,
            int pageNumber,
            ClaimsPrincipal user);

        Task<MarketplaceListing?> GetListingDetailsAsync(Guid id);

        Task<List<MarketplaceListing>> GetUserListingsAsync(ClaimsPrincipal user);

        Task<MarketplaceListingViewModel> GetCreateViewModelAsync();

        Task<Guid?> CreateListingAsync(MarketplaceListingViewModel model, ClaimsPrincipal user);

        Task<MarketplaceListingViewModel?> GetEditViewModelAsync(Guid id, ClaimsPrincipal user);

        Task<bool> UpdateListingAsync(MarketplaceListingViewModel model, ClaimsPrincipal user);

        Task<MarketplaceListing?> GetDeleteConfirmationAsync(Guid id, ClaimsPrincipal user);

        Task<bool> DeleteListingAsync(Guid id);

        Task<List<string>> UploadImagesAsync(IFormFileCollection files, Guid listingId);

        Task<bool> DeleteImageAsync(Guid imageId);
    }
}
