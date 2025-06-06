using Catering.Core.Contracts;
using Catering.Core.DTOs.Restaurant;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;

namespace Catering.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository repository;

        public RestaurantService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<int> CreateRestaurant(CreateRestaurantRequestDto restaurantDto)
        {
            var restaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                ContactEmail = restaurantDto.ContactEmail,
                PhoneNumber = restaurantDto.PhoneNumber,
                Address = restaurantDto.Address,
                OwnerId = restaurantDto.OwnerId,
                IsPublic = false,
            };

            await repository.AddAsync(restaurant);
            await repository.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
