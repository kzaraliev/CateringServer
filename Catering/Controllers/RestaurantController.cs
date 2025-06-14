using Catering.Core.Constants;
using Catering.Core.Contracts;
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

        public RestaurantController(IRestaurantService _restaurantService)
        {
            restaurantService = _restaurantService;
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
    }
}
