using Catering.Core.Contracts;
using Catering.Core.DTOs.Restaurant;
using Catering.Core.DTOs.WorkingDay;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository repository;
        private readonly IMenuService menuService;

        public RestaurantService(IRepository _repository, IMenuService _menuService)
        {
            repository = _repository;
            menuService = _menuService;
        }

        public async Task<int> CreateRestaurantAsync(CreateRestaurantRequestDto restaurantDto)
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

            await menuService.CreateDefaultMenuCategoryAsync(restaurant.Id);

            return restaurant.Id;
        }

        public async Task UpdateRestaurantAsync(UpdateRestaurantDto restaurantDto, string userId)
        {
            var restaurant = await ValidateOwnership(userId, restaurantDto.Id);

            if (restaurantDto.WorkingDays != null)
            {
                ValidateWorkingDays(restaurantDto.WorkingDays);
            }

            restaurant.Name = restaurantDto.Name ?? restaurant.Name;
            restaurant.Description = restaurantDto.Description ?? restaurant.Description;
            restaurant.PhoneNumber = restaurantDto.PhoneNumber ?? restaurant.PhoneNumber;
            restaurant.Address = restaurantDto.Address ?? restaurant.Address;
            restaurant.IsPublic = restaurantDto.IsPublic ?? restaurant.IsPublic;
            restaurant.ImageUrl = restaurantDto.ImageUrl ?? restaurant.ImageUrl;

            if (restaurantDto.DeliveryMethod.HasValue)
            {
                restaurant.SupportedDeliveryMethods = restaurantDto.DeliveryMethod.Value;
            }

            var existingWorkingDays = await repository
                .All<WorkingDay>()
                .Where(wd => wd.RestaurantId == restaurant.Id)
                .ToListAsync();

            if (existingWorkingDays.Any())
            {
                repository.RemoveRange(existingWorkingDays);
            }

            var newWorkingDays = (restaurantDto.WorkingDays ?? Enumerable.Empty<WorkingDayDto>()).Select(dayDto => new WorkingDay
            {
                Day = dayDto.Day,
                OpenTime = dayDto.IsClosed ? null : dayDto.OpenTime,
                CloseTime = dayDto.IsClosed ? null : dayDto.CloseTime,
                IsClosed = dayDto.IsClosed,
                RestaurantId = restaurantDto.Id
            }).ToList();

            await repository.AddRangeAsync(newWorkingDays);

            await repository.SaveChangesAsync();
        }

        private void ValidateWorkingDays(IEnumerable<WorkingDayDto> workingDays)
        {
            foreach (var day in workingDays)
            {
                if (day.IsClosed) continue;

                if (!day.OpenTime.HasValue || !day.CloseTime.HasValue)
                {
                    throw new ArgumentException($"OpenTime and CloseTime are required for open days ({day.Day}).");
                }

                if (day.OpenTime >= day.CloseTime)
                {
                    throw new ArgumentException($"OpenTime must be before CloseTime for {day.Day}");
                }

                if (day.OpenTime.Value.TotalHours < 0 || day.CloseTime.Value.TotalHours > 24)
                {
                    throw new ArgumentException($"Working hours must be within 0-24 range for {day.Day}");
                }
            }
        }

        private async Task<Restaurant> ValidateOwnership(string userId, int restaurantId)
        {
            var user = await repository.AllReadOnly<IdentityUser>()
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

            var restaurant = await repository.All<Restaurant>()
                .FirstOrDefaultAsync(r => r.Id == restaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            if (restaurant.OwnerId != user.Id)
            {
                throw new InvalidOperationException("You are not authorized to perform this action on the restaurant.");
            }

            return restaurant;
        }
    }
}
