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
        /// Gets or sets the optional message provided by the requester.
        /// </summary>
        [MaxLength(MessageMaxLength)]
        [Comment("Optional message from the requester")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the requester, if they are registered.
        /// </summary>
        [Comment("User ID of the requester (if logged in)")]
        public string? RequestedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the application user that made the request.
        /// </summary>
        [ForeignKey(nameof(RequestedByUserId))]
        public ApplicationUser? RequestedByUser { get; set; }

        /// <summary>
        /// Gets or sets the invitation token, used if the requester is not registered.
        /// </summary>
        [MaxLength(InviteTokenMaxLength)]
        [Comment("Invitation token for unregistered users")]
        public string? InvitationToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the invitation token.
        /// </summary>
        [Comment("Expiration timestamp for the invitation token")]
        public DateTime? TokenExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets whether the request has been approved.
        /// </summary>
        [Comment("Whether the request has been approved")]
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the request was approved.
        /// </summary>
        [Comment("Timestamp of approval")]
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// Gets or sets the associated restaurant created from the approved request.
        /// </summary>
        [Comment("Restaurant created from this request")]
        public Restaurant? Restaurant { get; set; }
    }
}
