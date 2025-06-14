using Catering.Core.Contracts;
using Catering.Core.DTOs.Queries;
using Catering.Core.DTOs.Restaurant;
using Catering.Core.DTOs.WorkingDay;
using Catering.Core.Utils;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
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

        public async Task<PagedResult<RestaurantsDto>> GetAllRestaurantsAsync(RestaurantQueryParametersDto queryParams)
        {
            var query = repository.AllReadOnly<Restaurant>();
            query = query.Where(r => r.IsPublic);
            query = query.Include(r => r.MenuCategories).ThenInclude(c => c.MenuItems);

            //IsOpen filter
            if (queryParams.IsOpen.HasValue && queryParams.IsOpen.Value)
            {
                var currentDay = DateTime.Now.DayOfWeek;
                var currentTime = DateTime.Now.TimeOfDay;

                query = query.Where(r => r.WorkingDays.Any(wd =>
                    wd.Day == currentDay &&
                    !wd.IsClosed &&
                    wd.OpenTime.HasValue && wd.CloseTime.HasValue &&
                    currentTime >= wd.OpenTime.Value &&
                    currentTime <= wd.CloseTime.Value));
            }

            //Delivery method filter
            if (queryParams.DeliveryMethods.HasValue)
            {
                query = query.Where(r => (r.SupportedDeliveryMethods & queryParams.DeliveryMethods.Value) != 0);
            }

            //Search
            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();

                query = query.Where(r =>
                    r.Description != null && r.Description.ToLower().Contains(term) ||
                    r.Address.ToLower().Contains(term) ||
                    r.Name.ToLower().Contains(term) ||
                    r.MenuCategories.Any(c =>
                        c.Name.ToLower().Contains(term) ||
                        c.MenuItems.Any(i => i.Name.ToLower().Contains(term))
                    )
                );
            }

            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);

            var response = await query.ToPagedResultAsync(
                queryParams.Page,
                queryParams.PageSize,
                r => new RestaurantsDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.ImageUrl,
                });

            return response;
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

        public async Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto, string userId)
        {
            var restaurant = await ValidateOwnership(userId, restaurantId);

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

            if (restaurantDto.WorkingDays != null)
            {
                var existingWorkingDays = await repository
                    .All<WorkingDay>()
                    .Where(wd => wd.RestaurantId == restaurant.Id)
                    .ToListAsync();

                if (existingWorkingDays.Any())
                {
                    repository.RemoveRange(existingWorkingDays);
                }

                var newWorkingDays = (restaurantDto.WorkingDays).Select(dayDto => new WorkingDay
                {
                    Day = dayDto.Day,
                    OpenTime = dayDto.IsClosed ? null : dayDto.OpenTime,
                    CloseTime = dayDto.IsClosed ? null : dayDto.CloseTime,
                    IsClosed = dayDto.IsClosed,
                    RestaurantId = restaurant.Id
                }).ToList();

                await repository.AddRangeAsync(newWorkingDays);
            }

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
            var restaurant = await repository.All<Restaurant>()
                .FirstOrDefaultAsync(r => r.Id == restaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            if (restaurant.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action on the restaurant.");
            }

            return restaurant;
        }
    }
}
