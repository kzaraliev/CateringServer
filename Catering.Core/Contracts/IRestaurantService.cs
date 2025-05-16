using Catering.Core.DTOs.Restaurant;

namespace Catering.Core.Contracts
{
    public interface IRestaurantService
    {
        Task<int> CreateRestaurant(CreateRestaurantRequestDto restaurantDto);
    }
}
