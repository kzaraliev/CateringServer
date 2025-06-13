using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository repository;

        public MenuService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task CreateMenuItemAsync(CreateMenuItemDto menuItemDto, string userId)
        {
            await ValidateOwnership(userId, menuItemDto.RestaurantId);

            var menuCategory = await repository.AllReadOnly<MenuCategory>()
                .FirstOrDefaultAsync(mc => mc.Id == menuItemDto.MenuCategoryId && mc.RestaurantId == menuItemDto.RestaurantId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuItemDto.MenuCategoryId} not found for Restaurant ID {menuItemDto.RestaurantId}.");

            var menuItem = new MenuItem
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                Price = menuItemDto.Price,
                ImageUrl = menuItemDto.ImageUrl,
                MenuCategoryId = menuItemDto.MenuCategoryId
            };

            await repository.AddAsync(menuItem);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int menuItemId, string userId)
        {
            var menuItem = await repository.FindAsync<MenuItem>(menuItemId)
                ?? throw new KeyNotFoundException($"MenuItem with ID {menuItemId} not found.");
            var menuCategory = await repository.AllReadOnly<MenuCategory>()
                .FirstOrDefaultAsync(mc => mc.Id == menuItem.MenuCategoryId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuItem.MenuCategoryId} not found.");

            await ValidateOwnership(userId, menuCategory.RestaurantId);

            repository.Remove(menuItem);
            await repository.SaveChangesAsync();
        }

        public async Task CreateMenuCategoryAsync(CreateMenuCategoryDto menuCategoryDto, string userId)
        {
            await ValidateOwnership(userId, menuCategoryDto.RestaurantId);

            var menuCategory = new MenuCategory
            {
                Name = menuCategoryDto.Name,
                Description = menuCategoryDto.Description,
                RestaurantId = menuCategoryDto.RestaurantId
            };
            await repository.AddAsync(menuCategory);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteMenuCategoryAsync(int menuCategoryId, string userId)
        {
            var menuCategory = await repository.AllReadOnly<MenuCategory>()
                .Include(mc => mc.MenuItems)
                .FirstOrDefaultAsync(mc => mc.Id == menuCategoryId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuCategoryId} not found.");

            if (menuCategory.MenuItems.Any())
            {
                throw new InvalidOperationException("Cannot delete a menu category that contains menu items. Please remove the items first.");
            }

            await ValidateOwnership(userId, menuCategory.RestaurantId);

            repository.Remove(menuCategory);
            await repository.SaveChangesAsync();
        }

        public async Task CreateDefaultMenuCategoryAsync(int restaurantId)
        {

            var menuCategory = new MenuCategory
            {
                Name = "Без категория",
                RestaurantId = restaurantId
            };

            await repository.AddAsync(menuCategory);
            await repository.SaveChangesAsync();
        }

        private async Task ValidateOwnership(string userId, int restaurantId)
        {
            var restaurant = await repository.AllReadOnly<Restaurant>()
                .FirstOrDefaultAsync(r => r.Id == restaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with ID {restaurantId} not found.");

            if (restaurant.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action on the restaurant.");
            }
        }
    }
}
