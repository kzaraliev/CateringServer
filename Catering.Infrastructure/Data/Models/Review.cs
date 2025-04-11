using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.ReviewConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a review left by a user for a restaurant.
    /// </summary>
    [Comment("Represents a review left by a user for a restaurant.")]
    public class Review
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        [Key]
        [Comment("Review Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the foreign key referencing the reviewed restaurant.
        /// </summary>
        [Required]
        [Comment("Foreign key to the reviewed restaurant.")]
        public int RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant associated with this review.
        /// </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the user who submitted the review.
        /// </summary>
        [Required]
        [Comment("Identifier of the user who submitted the review.")]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who submitted the review.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rating provided in the review.
        /// </summary>
        [Required]
        [Comment("Rating provided in the review.")]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the optional comment provided in the review.
        /// </summary>
        [MaxLength(CommentMaxLength)]
        [Comment("Optional comment provided in the review.")]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the review was created.
        /// </summary>
        [Required]
        [Comment("Date and time when the review was created.")]
        public DateTime CreatedAt { get; set; }
    }
}
