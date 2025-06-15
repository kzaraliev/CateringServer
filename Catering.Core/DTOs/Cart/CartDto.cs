using Catering.Core.DTOs.CartItem;

namespace Catering.Core.DTOs.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public decimal Subtotal { get; set; }
        public int TotalItems { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
