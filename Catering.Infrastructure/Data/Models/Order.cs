using Catering.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.OrderConstants;
using static Catering.Infrastructure.Constants.AddressConstants;

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
        /// This is null for guest orders.
        /// </summary>
        [Comment("Customer Identifier (nullable for guest orders)")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer who placed the order. Null if it's a guest order.
        /// </summary>
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser? Customer { get; set; }

        /// <summary>
        /// Gets or sets the email for a guest customer.
        /// </summary>
        [MaxLength(EmailMaxLength)]
        [Comment("Email for guest customer")]
        public string? GuestEmail { get; set; }

        /// <summary>
        /// Gets or sets the phone number for a guest customer.
        /// </summary>
        [MaxLength(PhoneMaxLength)]
        [Comment("Phone number for guest customer")]
        public string? GuestPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name for a guest customer.
        /// </summary>
        [MaxLength(GuestNameMaxLength)] // Assuming FirstNameMaxLength constant
        [Comment("First name for guest customer")]
        public string? GuestName { get; set; }

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
        /// Gets or sets the type of order (Delivery or Pickup).
        /// </summary>
        [Required]
        [Comment("Type of order: Delivery or Pickup")]
        public required OrderType OrderType { get; set; }

        /// <summary>
        /// Gets or sets the street address. Nullable if OrderType is Pickup.
        /// </summary>
        [MaxLength(StreetMaxLength)]
        [Comment("Street address")]
        public string? Street { get; set; }

        /// <summary>
        /// Gets or sets the city. Nullable if OrderType is Pickup.
        /// </summary>
        [MaxLength(CityMaxLength)]
        [Comment("City")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the ZIP/postal code. Nullable if OrderType is Pickup.
        /// </summary>
        [MaxLength(ZipCodeMaxLength)]
        [Comment("ZIP/Postal code")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the requested delivery or pickup time.
        /// </summary>
        [Required]
        [Comment("Requested delivery or pickup time")]
        public DateTime RequestedDeliveryTime { get; set; }

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
        /// Gets or sets the delivery fee.
        /// </summary>
        [Required]
        [Comment("Delivery fee")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        [Required]
        [Comment("Total amount after tax, discount, and delivery fee")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the payment method used for the order.
        /// </summary>
        [Required]
        [Comment("Payment method used for the order")]
        public required Enums.PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets any additional notes or instructions for the order.
        /// </summary>
        [MaxLength(NotesMaxLength)]
        [Comment("Additional notes or instructions")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in this order.
        /// </summary>
        [Comment("Collection of items in this order")]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
} 