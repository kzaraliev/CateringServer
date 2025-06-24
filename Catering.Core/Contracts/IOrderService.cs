using Catering.Core.DTOs.Order;

namespace Catering.Core.Contracts
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(string? userId, Guid? cartId, PlaceOrderRequestDto request);
    }
}
