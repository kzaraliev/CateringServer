using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.OrderItemConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an item within an order in the catering system.
    /// </summary>
    [Comment("Represents an item within an order in the catering system.")]
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order item.
        /// </summary>
        [Key]
        [Comment("Order Item Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the order this item belongs to.
        /// </summary>
        [Required]
        [Comment("Order Identifier")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order this item belongs to.
        /// </summary>
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Gets or sets the menu item.
        /// </summary>
        [Required]
        [StringLength(NameMaxLength)]
        [Comment("Name of the menu item")]
        public required string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the image of the menu item.
        /// </summary>
        [Comment("Image of the menu item")]
        [StringLength(ImageUrlMaxLength)]
        public string? ItemImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the menu item being ordered.
        /// </summary>
        [Required]
        [Comment("Quantity of the menu item")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the menu item at the time of ordering.
        /// </summary>
        [Required]
        [Comment("Unit price at the time of ordering")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price for this order item (quantity * unit price).
        /// </summary>
        [Required]
        [Comment("Total price for this order item")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
} 