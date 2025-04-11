using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.MenuCategoryConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a category of menu items for a restaurant.
    /// </summary>
    [Comment("Represents a category of menu items for a restaurant.")]
    public class MenuCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the menu category.
        /// </summary>
        [Key]
        [Comment("Menu Category Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu category.
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Name of the menu category")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets an optional description for the menu category.
        /// </summary>
        [MaxLength(DescriptionMaxLength)]
        [Comment("Optional description for the menu category")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the foreign key referencing the restaurant to which this category belongs.
        /// </summary>
        [Required]
        [Comment("Foreign key to the restaurant.")]
        public int RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant associated with this menu category.
        /// </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of menu items under this category.
        /// </summary>
        [Comment("Menu items under this category")]
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
