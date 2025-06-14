using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Queries;

namespace Catering.Core.Contracts
{
    public interface IMenuService
    {
        Task CreateMenuItemAsync(int restaurantId, CreateMenuItemDto menuItemDto, string userId);
        Task CreateMenuCategoryAsync(int restaurantId, CreateMenuCategoryDto menuCategoryDto, string userId);
        Task CreateDefaultMenuCategoryAsync(int restaurantId);
        Task DeleteMenuItemAsync(int restaurantId, int menuItemId, string userId);
        Task DeleteMenuCategoryAsync(int restaurantId, int menuCategoryId, string userId);
        Task UpdateMenuItemAsync(int restaurantId, int menuItemId, UpdateMenuItemDto menuItemDto, string userId);
        Task UpdateMenuCategoryAsync(int restaurantId, int menuCategoryId, UpdateMenuCategoryDto menuCategoryDto, string userId);
        Task<PagedResult<MenuItemsDto>> GetAllMenuItemsForRestaurantAsync(int restaurantId, MenuItemQueryParametersDto queryParams);
        Task<PagedResult<MenuCategoriesDto>> GetAllMenuCategoriesForRestaurantAsync(int restaurantId, MenuCategoryQueryParametersDto queryParams);
    }
}
