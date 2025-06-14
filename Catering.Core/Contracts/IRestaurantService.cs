using Catering.Core.DTOs.Queries;
using Catering.Core.DTOs.Restaurant;

namespace Catering.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<int> CreateRestaurantAsync(CreateRestaurantRequestDto restaurantDto);
        Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto, string userId);
        Task<PagedResult<RestaurantsDto>> GetAllRestaurantsAsync(RestaurantQueryParametersDto queryParams);
    }
}
