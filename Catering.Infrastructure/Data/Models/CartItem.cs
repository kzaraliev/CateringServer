using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an individual item within a shopping cart.
    /// </summary>
    [Comment("Represents an individual item within a shopping cart.")]
    public class CartItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart item.
        /// </summary>
        [Key]
        [Comment("Unique identifier for the cart item.")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the menu item in the cart.
        /// </summary>
        [Required]
        [Comment("Quantity of the menu item in the cart.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated Cart.
        /// </summary>
        [Required]
        [Comment("Foreign key to the associated Cart.")]
        public Guid CartId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated Cart.
        /// </summary>
        [ForeignKey(nameof(CartId))]
        [Comment("Navigation property to the associated Cart.")]
        public Cart Cart { get; set; } = null!;

        /// <summary>
        /// Gets or sets the foreign key to the associated MenuItem.
        /// </summary>
        [Required]
        [Comment("Foreign key to the associated MenuItem.")]
        public int MenuItemId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated MenuItem.
        /// </summary>
        [ForeignKey(nameof(MenuItemId))]
        [Comment("Navigation property to the associated MenuItem.")]
        public MenuItem MenuItem { get; set; } = null!;
    }
}
