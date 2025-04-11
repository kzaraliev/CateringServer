using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.OrderConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an order in the catering system.
    /// </summary>
    [Comment("Represents an order in the catering system.")]
    public class Order
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        [Key]
        [Comment("Order Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was placed.
        /// </summary>
        [Required]
        [Comment("Date and time when the order was placed")]
        public required DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer who placed the order.
        /// </summary>
        [Required]
        [Comment("Customer Identifier")]
        public required string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer who placed the order.
        /// </summary>
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the restaurant that receives the order.
        /// </summary>
        [Required]
        [Comment("Restaurant Identifier")]
        public int RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant that receives the order.
        /// </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status of the order (e.g., Pending, Confirmed, Preparing, Ready, Delivered, Cancelled).
        /// </summary>
        [Required]
        [Comment("Order status")]
        public required OrderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the delivery method (e.g., Pickup, Delivery).
        /// </summary>
        [Required]
        [Comment("Delivery method")]
        public required DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the delivery address.
        /// </summary>
        [Comment("Delivery Address Identifier")]
        public int? DeliveryAddressId { get; set; }

        /// <summary>
        /// Gets or sets the delivery address.
        /// </summary>
        [ForeignKey(nameof(DeliveryAddressId))]
        public Address? DeliveryAddress { get; set; }

        /// <summary>
        /// Gets or sets the requested delivery or pickup time.
        /// </summary>
        [Comment("Requested delivery or pickup time")]
        public DateTime? RequestedDeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets the actual delivery or pickup time.
        /// </summary>
        [Comment("Actual delivery or pickup time")]
        public DateTime? ActualDeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount before tax, discount, and delivery fee.
        /// </summary>
        [Required]
        [Comment("Subtotal amount before tax, discount, and delivery fee")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        [Required]
        [Comment("Tax amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the delivery fee.
        /// </summary>
        [Comment("Delivery fee")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        [Comment("Discount amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the total amount after tax, discount, and delivery fee.
        /// </summary>
        [Required]
        [Comment("Total amount after tax, discount, and delivery fee")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets any additional notes or instructions for the order.
        /// </summary>
        [MaxLength(NotesMaxLength)]
        [Comment("Additional notes or instructions")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the coupon applied to this order.
        /// </summary>
        [Comment("Coupon Identifier")]
        public int? CouponId { get; set; }

        /// <summary>
        /// Gets or sets the coupon applied to this order.
        /// </summary>
        [ForeignKey(nameof(CouponId))]
        public Coupon? Coupon { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in this order.
        /// </summary>
        [Comment("Collection of items in this order")]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Gets or sets the payment for this order.
        /// </summary>
        public Payment? Payment { get; set; }
    }

    /// <summary>
    /// Represents the possible order statuses.
    /// </summary>
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        Ready = 3,
        InDelivery = 4,
        Delivered = 5,
        Completed = 6,
        Cancelled = 7,
        Refunded = 8
    }

    /// <summary>
    /// Represents the possible delivery methods for an order.
    /// </summary>
    public enum DeliveryMethod
    {
        /// <summary>
        /// Customer picks up the order from the restaurant.
        /// </summary>
        Pickup = 0,
        
        /// <summary>
        /// Restaurant delivers the order to the customer.
        /// </summary>
        RestaurantDelivery = 1
    }
} 