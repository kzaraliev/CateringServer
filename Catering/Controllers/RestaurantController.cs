using Catering.Core.Contracts;
using Catering.Core.DTOs.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService _restaurantService)
        {
            restaurantService = _restaurantService;
        }

        [HttpPost("CreateRestaurant")]
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
    }
}
