using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.MenuItemConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an individual menu item or product offered by the restaurant.
    /// </summary>
    [Comment("Represents an individual menu item or product offered by the restaurant.")]
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the menu item.
        /// </summary>
        [Key]
        [Comment("Menu Item Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu item.
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Name of the menu item")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the menu item.
        /// </summary>
        [MaxLength(DescriptionMaxLength)]
        [Comment("Description of the menu item")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the menu item.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Price of the menu item")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the menu item is currently available for order.
        /// Defaults to true.
        /// </summary>
        [Required]
        [Comment("Indicates if the menu item is currently available for order.")]
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Gets or sets the image URL for the menu item.
        /// </summary>
        [Url]
        [MaxLength(ImageUrlMaxLength)]
        [Comment("Image URL for the menu item")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the foreign key referencing the associated menu category.
        /// </summary>
        [Required]
        [Comment("Foreign key to the associated menu category.")]
        public int MenuCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the associated menu category for the menu item.
        /// </summary>
        [ForeignKey(nameof(MenuCategoryId))]
        public MenuCategory MenuCategory { get; set; } = null!;
    }
}
