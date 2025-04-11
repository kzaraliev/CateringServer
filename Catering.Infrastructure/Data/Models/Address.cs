using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.AddressConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a user address in the system for delivery or pickup.
    /// </summary>
    [Comment("Represents a user address in the system for delivery or pickup.")]
    public class Address
    {
        /// <summary>
        /// Gets or sets the unique identifier for the address.
        /// </summary>
        [Key]
        [Comment("Address Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a friendly name for this address (e.g., "Home", "Work").
        /// </summary>
        [MaxLength(AddressNameMaxLength)]
        [Comment("Friendly name for this address")]
        public string? AddressName { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        [Required]
        [MaxLength(StreetMaxLength)]
        [Comment("Street address")]
        public required string Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [Required]
        [MaxLength(CityMaxLength)]
        [Comment("City")]
        public required string City { get; set; }

        /// <summary>
        /// Gets or sets the ZIP/postal code.
        /// </summary>
        [Required]
        [MaxLength(ZipCodeMaxLength)]
        [Comment("ZIP/Postal code")]
        public required string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [Required]
        [MaxLength(CountryMaxLength)]
        [Comment("Country")]
        public required string Country { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who owns this address.
        /// </summary>
        [Required]
        [Comment("User Identifier")]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who owns this address.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of orders delivered to this address.
        /// </summary>
        [Comment("Collection of orders delivered to this address")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 