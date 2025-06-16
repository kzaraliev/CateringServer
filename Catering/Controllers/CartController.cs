using Catering.Core.Contracts;
using Catering.Core.DTOs.Cart;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService _cartService)
        {
            cartService = _cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] Guid? cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await cartService.GetOrCreateCartAsync(cartId, userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromQuery] Guid? cartId, [FromBody] AddItemToCartRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await cartService.AddItemToCartAsync(cartId, userId, request);
            return Ok(cart);
        }

        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromQuery] Guid? cartId, [FromRoute] int cartItemId, [FromBody] UpdateCartItemQuantityRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await cartService.UpdateCartItemQuantityAsync(cartId, userId, cartItemId, request);
            return Ok(cart);
        }
    }
}
