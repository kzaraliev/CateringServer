using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Catering.Core.DTOs.Restaurant;
using Catering.Infrastructure.Data.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantRequestDto restaurantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await restaurantService.CreateRestaurantAsync(restaurantDto);
                return StatusCode(201, new { Message = "Restaurant created successfully", RestaurantId = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while processing your request");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(UpdateRestaurantDto restaurantDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await restaurantService.UpdateRestaurantAsync(restaurantDto, userId);
            return NoContent();
        }

        [HttpPost("menu-items")]
        public async Task<IActionResult> CreateMenuItem(CreateMenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await menuService.CreateMenuItemAsync(menuItemDto, userId);
            return StatusCode(201, new { Message = "Menu item created successfully" });
        }

        [HttpPut("menu-items/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItemDto menuItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await menuService.UpdateMenuItemAsync(id, menuItemDto, userId);
            return NoContent();
        }

        [HttpDelete("menu-items/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid menu item ID.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await menuService.DeleteMenuItemAsync(id, userId);
            return NoContent();
        }

        [HttpPost("menu-categories")]
        public async Task<IActionResult> CreateMenuCategory(CreateMenuCategoryDto menuCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await menuService.CreateMenuCategoryAsync(menuCategoryDto, userId);
            return StatusCode(201, new { Message = "Menu category created successfully" });
        }

        [HttpDelete("menu-categories/{id}")]
        public async Task<IActionResult> DeleteMenuCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid menu category ID.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await menuService.DeleteMenuCategoryAsync(id, userId);
            return NoContent();
        }
    }
}
