using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;

namespace Catering.Core.Contracts
{
    public interface IMenuService
    {
        Task CreateMenuItemAsync(CreateMenuItemDto menuItemDto, string userId);
        Task CreateMenuCategoryAsync(CreateMenuCategoryDto menuCategoryDto, string userId);
        Task CreateDefaultMenuCategoryAsync(int restaurantId);
    }
}
