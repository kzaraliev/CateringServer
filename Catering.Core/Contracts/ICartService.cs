using Catering.Core.DTOs.Cart;
using Catering.Infrastructure.Data.Models;

namespace Catering.Core.Contracts
{
    public interface ICartService
    {
        Task<CartDto> GetOrCreateCartAsync(Guid? cartId, string? userId);
        Task<CartDto> AddItemToCartAsync(Guid? cartId, string? userId, AddItemToCartRequestDto request);
        Task<CartDto> UpdateCartItemQuantityAsync(Guid? cartId, string? userId, int cartItemId, UpdateCartItemQuantityRequestDto request);
        Task<CartDto> RemoveItemFromCartAsync(Guid? cartId, string? userId, int cartItemId);
        Task<CartDto> ClearCartAsync(Guid? cartId, string? userId);
        Task PerformCartCleanupAsync(Cart cart);
    }
}
