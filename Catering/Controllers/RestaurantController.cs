using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Controllers
{
    [Route("api/Restaurants")]
    [ApiController]
    [Authorize(Roles = RoleNames.RestaurantOwner)]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService restaurantService;
        private readonly IMenuService menuService;

        public RestaurantController(IRestaurantService _restaurantService, IMenuService _menuService)
        {
            restaurantService = _restaurantService;
            menuService = _menuService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRestaurants([FromQuery] RestaurantQueryParametersDto queryParams)
        {
            var restaurants = await restaurantService.GetAllRestaurantsAsync(queryParams);
            return Ok(restaurants);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurantDto restaurantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await restaurantService.UpdateRestaurantAsync(id, restaurantDto, userId);
            return NoContent();
        }

        [HttpGet("{restaurantId}/menu/items")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuItems(int restaurantId, [FromQuery] MenuItemQueryParametersDto queryParams)
        {
            var menuItems = await menuService.GetAllMenuItemsForRestaurantAsync(restaurantId, queryParams);
            return Ok(menuItems);
        }

        [HttpPost("{restaurantId}/menu/items")]
        public async Task<IActionResult> CreateMenuItem(int restaurantId, CreateMenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.CreateMenuItemAsync(restaurantId, menuItemDto, userId);
            return StatusCode(201, new { Message = "Menu item created successfully" });
        }

        [HttpPut("{restaurantId}/menu/items/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int restaurantId, int id, UpdateMenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.UpdateMenuItemAsync(restaurantId, id, menuItemDto, userId);
            return NoContent();
        }

        [HttpDelete("{restaurantId}/menu/items/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int restaurantId, int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.DeleteMenuItemAsync(restaurantId, id, userId);
            return NoContent();
        }

        [HttpGet("{restaurantId}/menu/categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuCategories(int restaurantId, [FromQuery] MenuCategoryQueryParametersDto queryParams)
        {
            var menuCategories = await menuService.GetAllMenuCategoriesForRestaurantAsync(restaurantId, queryParams);
            return Ok(menuCategories);
        }

        [HttpPost("{restaurantId}/menu/categories")]
        public async Task<IActionResult> CreateMenuCategory(int restaurantId, CreateMenuCategoryDto menuCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.CreateMenuCategoryAsync(restaurantId, menuCategoryDto, userId);
            return StatusCode(201, new { Message = "Menu category created successfully" });
        }

        [HttpPut("{restaurantId}/menu/categories/{id}")]
        public async Task<IActionResult> UpdateMenuCategory(int restaurantId, int id, UpdateMenuCategoryDto menuCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.UpdateMenuCategoryAsync(restaurantId, id, menuCategoryDto, userId);
            return NoContent();
        }

        [HttpDelete("{restaurantId}/menu/categories/{id}")]
        public async Task<IActionResult> DeleteMenuCategory(int restaurantId, int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await menuService.DeleteMenuCategoryAsync(restaurantId, id, userId);
            return NoContent();
        }
    }
}
