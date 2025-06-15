using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a shopping cart, which can belong to an authenticated user or be a guest cart.
    /// </summary>
    [Comment("Represents a shopping cart.")]
    public class Cart
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart.
        /// </summary>
        [Key]
        [Comment("Unique identifier for the cart.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the authenticated user who owns this cart.
        /// This will be null for guest carts.
        /// </summary>
        [Comment("ID of the authenticated user who owns this cart. Null for guest carts.")]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user of the cart.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the cart was created.
        /// </summary>
        [Required]
        [Comment("UTC date and time when the cart was created.")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC date and time when the cart was last modified.
        /// </summary>
        [Comment("UTC date and time when the cart was last modified.")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the collection of items within this cart.
        /// </summary>
        [Comment("Collection of items within this cart.")]
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
