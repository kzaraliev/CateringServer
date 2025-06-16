namespace Catering.Core.DTOs.CartItem
{
    public class CartItemDto
    {
        public required int Id { get; set; }
        public required int MenuItemId { get; set; }
        public required string Name { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
