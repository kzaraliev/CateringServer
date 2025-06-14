using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Queries;

namespace Catering.Core.Contracts
{
    public interface IMenuService
    {
        Task CreateMenuItemAsync(CreateMenuItemDto menuItemDto, string userId);
        Task CreateMenuCategoryAsync(CreateMenuCategoryDto menuCategoryDto, string userId);
        Task CreateDefaultMenuCategoryAsync(int restaurantId);
        Task DeleteMenuItemAsync(int menuItemId, string userId);
        Task DeleteMenuCategoryAsync(int menuCategoryId, string userId);
        Task UpdateMenuItemAsync(int menuItemId, UpdateMenuItemDto menuItemDto, string userId);
        Task UpdateMenuCategoryAsync(int menuCategoryId, UpdateMenuCategoryDto menuCategoryDto, string userId);
        Task<PagedResult<MenuItemsDto>> GetAllMenuItemsForRestaurantAsync(int restaurantId, MenuItemQueryParametersDto queryParams);
    }
}
