using Catering.Core.Contracts;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var cart = await cartService.GetOrCreateCartAsync(cartId, userId);
            return Ok(cart);
        }
    }
}
