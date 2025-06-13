using Catering.Core.DTOs.Queries;
using Catering.Core.DTOs.Restaurant;

namespace Catering.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<int> CreateRestaurantAsync(CreateRestaurantRequestDto restaurantDto);
        Task UpdateRestaurantAsync(UpdateRestaurantDto restaurantDto, string userId);
        Task<PagedResult<RestaurantsDto>> GetAllRestaurantsAsync(RestaurantQueryParametersDto queryParams);
    }
}
