using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
        public required string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the collection of orders delivered to this address.
        /// </summary>
        [Comment("Collection of orders delivered to this address")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 