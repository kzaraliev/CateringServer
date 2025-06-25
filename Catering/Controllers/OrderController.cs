using Catering.Core.Contracts;
using Catering.Core.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] Guid? cartId, PlaceOrderRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await orderService.PlaceOrderAsync(userId, cartId, request);
            return Ok(order);
        }

        [HttpPost("cancel-order")]
        public async Task<IActionResult> CancelOrder(CancelOrderRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await orderService.CancelOrderAsync(request.OrderId, userId, request.GuestEmail);
            return Ok(result);
        }
    }
}
