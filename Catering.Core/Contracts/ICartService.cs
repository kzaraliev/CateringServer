using Catering.Core.DTOs.Cart;

namespace Catering.Core.Contracts
{
    public interface ICartService
    {
        Task<CartDto> GetOrCreateCartAsync(Guid? cartId, string? userId);
        Task<CartDto> AddItemToCartAsync(Guid? cartId, string? userId, AddItemToCartRequestDto request);
    }
}
