using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.CouponConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a discount coupon that can be applied to orders.
    /// </summary>
    [Comment("Represents a discount coupon that can be applied to orders.")]
    public class Coupon
    {
        /// <summary>
        /// Gets or sets the unique identifier for the coupon.
        /// </summary>
        [Key]
        [Comment("Coupon Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the coupon code that users will enter.
        /// </summary>
        [Required]
        [MaxLength(CodeMaxLength)]
        [Comment("Coupon code")]
        public required string Code { get; set; }

        /// <summary>
        /// Gets or sets the description of the coupon.
        /// </summary>
        [MaxLength(DescriptionMaxLength)]
        [Comment("Coupon description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the discount amount in currency (if not percentage-based).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Discount amount in currency")]
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage (if not amount-based).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Discount percentage")]
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the minimum order amount required to use this coupon.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Minimum order amount required")]
        public decimal? MinimumOrderAmount { get; set; }

        /// <summary>
        /// Gets or sets the maximum discount amount when using percentage-based discount.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Maximum discount amount for percentage-based coupons")]
        public decimal? MaximumDiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the start date of the coupon validity period.
        /// </summary>
        [Required]
        [Comment("Start date of the coupon validity period")]
        public required DateTime ValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the end date of the coupon validity period.
        /// </summary>
        [Required]
        [Comment("End date of the coupon validity period")]
        public required DateTime ValidTo { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of times this coupon can be used.
        /// </summary>
        [Comment("Maximum usage count")]
        public int? MaxUsageCount { get; set; }

        /// <summary>
        /// Gets or sets the current usage count of this coupon.
        /// </summary>
        [Comment("Current usage count")]
        public int UsageCount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the restaurant that issued this coupon.
        /// </summary>
        [Required]
        [Comment("Restaurant Identifier")]
        public int RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant that issued this coupon.
        /// </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of orders where this coupon was applied.
        /// </summary>
        [Comment("Collection of orders where this coupon was applied")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 