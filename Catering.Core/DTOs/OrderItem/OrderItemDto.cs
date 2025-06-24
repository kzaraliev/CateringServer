namespace Catering.Core.DTOs.OrderItem
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int? OriginalMenuItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string? ItemImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
