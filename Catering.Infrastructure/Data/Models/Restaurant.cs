﻿using Catering.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.RestaurantConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a restaurant in the catering platform.
    /// </summary>
    [Comment("Represents a restaurant in the catering platform.")]
    public class Restaurant
    {
        /// <summary>
        /// Gets or sets the unique identifier for the restaurant.
        /// </summary>
        [Key]
        [Comment("Restaurant Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the restaurant.
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Restaurant Name")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the restaurant.
        /// </summary>
        [MaxLength(DescriptionMaxLength)]
        [Comment("Restaurant Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the contact email address of the restaurant.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(EmailMaxLength)]
        [Comment("Restaurant Email Address")]
        public required string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the restaurant.
        /// </summary>
        [Required]
        [Phone]
        [MaxLength(PhoneNumberMaxLength)]
        [Comment("Restaurant Phone Number")]
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the physical address of the restaurant.
        /// </summary>
        [Required]
        [MaxLength(AddressMaxLength)]
        [Comment("Restaurant Address")]
        public required string Address { get; set; }

        /// <summary>
        /// Gets or sets the public status of the restaurant.
        /// </summary>
        [Required]
        [Comment("Is restaurant public")]
        public required bool IsPublic { get; set; } = false;

        /// <summary>
        /// Gets or sets the delivery fee charged by the restaurant.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Comment("Delivery fee of the restaurant")]
        public required decimal DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets the URL address of the restaurant image.
        /// </summary>
        [Url]
        [MaxLength(ImageUrlMaxLength)]
        [Comment("Restaurant Image URL Address")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the delivery methods supported by the restaurant.
        /// </summary>
        [Comment("Delivery methods supported by the restaurant")]
        public RestaurantDeliveryMethods SupportedDeliveryMethods { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the restaurant owner.
        /// </summary>
        [Comment("Owner Identifier")]
        public string? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner of the restaurant.
        /// </summary>
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of menu categories offered by the restaurant.
        /// </summary>
        [Comment("Collection of menu categories offered by the restaurant.")]
        public ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();

        /// <summary>
        /// Gets or sets the collection of reviews for the restaurant.
        /// </summary>
        [Comment("Collection of reviews for the restaurant.")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        /// <summary>
        /// Gets or sets the collection of orders received by the restaurant.
        /// </summary>
        [Comment("Collection of orders received by the restaurant.")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Gets or sets the collection of coupons issued by the restaurant.
        /// </summary>
        [Comment("Collection of coupons issued by the restaurant.")]
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

        /// <summary>
        /// Gets or sets the collection of working days for the restaurant.
        /// </summary>
        [Comment("Collection of working days for the restaurant")]
        public ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
    }
}
