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
            foreach (var dayDto in restaurantDto.WorkingDays)
            {
                if (!Enum.IsDefined(typeof(DayOfWeek), dayDto.Day))
                {
                    throw new ArgumentException($"Invalid day of the week: {dayDto.Day}");
                }

                if (!dayDto.IsClosed)
                {
                    if (!dayDto.OpenTime.HasValue || !dayDto.CloseTime.HasValue)
                    {
                        throw new ArgumentException($"OpenTime and CloseTime are required for open days ({dayDto.Day}).");
                    }

                    if (dayDto.OpenTime.Value >= dayDto.CloseTime.Value)
                    {
                        throw new ArgumentException(
                            $"OpenTime must be before CloseTime for {dayDto.Day}");
                    }

                    if (dayDto.OpenTime.Value.TotalHours < 0 || dayDto.CloseTime.Value.TotalHours > 24)
                    {
                        throw new ArgumentException(
                            $"Working hours must be within 0-24 range for {dayDto.Day}");
                    }
                }
            }


            var restaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                Description = restaurantDto.Description,
                ContactEmail = restaurantDto.ContactEmail,
                PhoneNumber = restaurantDto.PhoneNumber,
                Address = restaurantDto.Address,
                ImageUrl = restaurantDto.ImageUrl,
                SupportedDeliveryMethods = restaurantDto.SupportedDeliveryMethods,
                OwnerId = restaurantDto.OwnerId,
            };

            await repository.AddAsync(restaurant);
            await repository.SaveChangesAsync();

            var workingDays = restaurantDto.WorkingDays.Select(dayDto => new WorkingDay
            {
                Day = dayDto.Day,
                OpenTime = dayDto.IsClosed ? null : dayDto.OpenTime,
                CloseTime = dayDto.IsClosed ? null : dayDto.CloseTime,
                IsClosed = dayDto.IsClosed,
                RestaurantId = restaurant.Id
            }).ToList();

            await repository.AddRangeAsync(workingDays);
            await repository.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
