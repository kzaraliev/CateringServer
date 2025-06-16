using Catering.Core.DTOs.CartItem;

namespace Catering.Core.DTOs.Cart
{
    public class CartDto
    {
        public required Guid Id { get; set; }
        public string? UserId { get; set; }
        public required decimal Subtotal { get; set; }
        public required int TotalItems { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
