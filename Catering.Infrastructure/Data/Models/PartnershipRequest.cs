using Catering.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.RequestConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a request for a restaurant partnership in the platform.
    /// </summary>
    [Comment("Represents a request for a restaurant partnership in the platform.")]
    public class PartnershipRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the partnership request.
        /// </summary>
        [Key]
        [Comment("Partnership Request Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the restaurant being requested.
        /// </summary>
        [Required]
        [MaxLength(RestaurantNameMaxLength)]
        [Comment("Name of the restaurant in the request")]
        public required string RestaurantName { get; set; }

        /// <summary>
        /// Gets or sets the contact email of the requester.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(EmailMaxLength)]
        [Comment("Email of the person requesting the partnership")]
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
        /// Gets or sets the optional message provided by the requester.
        /// </summary>
        [MaxLength(MessageMaxLength)]
        [Comment("Optional message from the requester")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the status of the partner request.
        /// </summary>
        [Comment("The status of the partner request.")]
        public PartnershipRequestStatus Status { get; set; } = PartnershipRequestStatus.Pending;

        /// <summary>
        /// Gets or sets the timestamp when the request was processed (approved or rejected).
        /// </summary>
        [Comment("Timestamp of request processing")]
        public DateTime? ProcessedAt { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the related restaurant.
        /// </summary>
        [Comment("Foreign key to the related restaurant created from this request")]
        public int? RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;
    }
}
