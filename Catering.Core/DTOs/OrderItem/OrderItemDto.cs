namespace Catering.Core.DTOs.OrderItem
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
