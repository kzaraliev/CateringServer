using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Queries;
using Catering.Core.Utils;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
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

        public async Task<PagedResult<MenuItemsDto>> GetAllMenuItemsForRestaurantAsync(int restaurantId, MenuItemQueryParametersDto queryParams)
        {
            var query = repository.AllReadOnly<MenuItem>();

            query = query.Where(mi => mi.MenuCategory.RestaurantId == restaurantId);

            //Search
            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                string term = queryParams.SearchTerm.Trim().ToLower();

                query = query.Where(mi =>
                    mi.Description != null && mi.Description.ToLower().Contains(term) ||
                    mi.Name.ToLower().Contains(term) ||
                    mi.MenuCategory.Name.ToLower().Contains(term)
                    );
            }

            query = query.ApplySorting(queryParams.SortBy, queryParams.SortDescending);

            var response = await query.ToPagedResultAsync(
                queryParams.Page,
                queryParams.PageSize,
                mi => new MenuItemsDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    Description = mi.Description,
                    ImageUrl = mi.ImageUrl,
                    MenuCategoryId = mi.MenuCategoryId,
                    Price = mi.Price,
                });

            return response;
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

        public async Task UpdateMenuItemAsync(int menuItemId, UpdateMenuItemDto menuItemDto, string userId)
        {
            var menuItem = await repository.All<MenuItem>()
                .Include(mi => mi.MenuCategory)
                .FirstOrDefaultAsync(mi => mi.Id == menuItemId)
                ?? throw new KeyNotFoundException($"MenuItem with ID {menuItemId} not found.");

            var originalRestaurantId = menuItem.MenuCategory.RestaurantId;

            await ValidateOwnership(userId, originalRestaurantId);

            menuItem.Name = menuItemDto.Name ?? menuItem.Name;
            menuItem.Description = menuItemDto.Description ?? menuItem.Description;
            menuItem.Price = menuItemDto.Price ?? menuItem.Price;
            menuItem.ImageUrl = menuItemDto.ImageUrl ?? menuItem.ImageUrl;

            if (menuItemDto.MenuCategoryId.HasValue && menuItemDto.MenuCategoryId.Value != menuItem.MenuCategoryId)
            {
                int newMenuCategoryId = menuItemDto.MenuCategoryId.Value;

                var targetMenuCategory = await repository.AllReadOnly<MenuCategory>()
                    .FirstOrDefaultAsync(mc => mc.Id == newMenuCategoryId && mc.RestaurantId == originalRestaurantId);

                if (targetMenuCategory == null)
                {
                    throw new KeyNotFoundException($"MenuCategory with ID {newMenuCategoryId} not found or does not belong to the current restaurant.");
                }

                menuItem.MenuCategoryId = newMenuCategoryId;
            }

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

        public async Task UpdateMenuCategoryAsync(int menuCategoryId, UpdateMenuCategoryDto menuCategoryDto, string userId)
        {
            var menuCategory = await repository.All<MenuCategory>()
                .FirstOrDefaultAsync(mc => mc.Id == menuCategoryId)
                ?? throw new KeyNotFoundException($"MenuCategory with ID {menuCategoryId} not found.");

            await ValidateOwnership(userId, menuCategory.RestaurantId);

            menuCategory.Name = menuCategoryDto.Name ?? menuCategory.Name;
            menuCategory.Description = menuCategoryDto.Description ?? menuCategory.Description;

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
