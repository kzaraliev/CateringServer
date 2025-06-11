using Catering.Core.DTOs.Restaurant;

namespace Catering.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<int> CreateRestaurantAsync(CreateRestaurantRequestDto restaurantDto);
        Task UpdateRestaurantAsync(UpdateRestaurantDto restaurantDto, string userId);
    }
}
