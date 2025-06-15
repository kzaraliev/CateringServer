namespace Catering.Core.DTOs.CartItem
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
