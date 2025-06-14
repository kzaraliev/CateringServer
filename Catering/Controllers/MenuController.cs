using Catering.Core.Contracts;
using Catering.Core.DTOs.MenuCategory;
using Catering.Core.DTOs.MenuItem;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService _menuService)
        {
            menuService = _menuService;
        }

        [HttpPost("items")]
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

        [HttpPut("items/{id}")]
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

        [HttpDelete("items/{id}")]
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

        [HttpPost("categories")]
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

        [HttpDelete("categories/{id}")]
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
