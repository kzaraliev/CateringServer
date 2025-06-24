using Catering.Core.DTOs.OrderItem;
using Catering.Infrastructure.Data.Enums;

namespace Catering.Core.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }

        // Guest info if CustomerId is null
        public string? GuestEmail { get; set; }
        public string? GuestPhoneNumber { get; set; }
        public string? GuestName { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType OrderType { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public DateTime RequestedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? DeliveryFee { get; set; }
        public decimal OrderTotal { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
