using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a refresh token used for renewing JWT tokens.
    /// </summary>
    [Comment("Represents a refresh token used for renewing JWT tokens.")]
    public class RefreshToken
    {
        /// <summary>
        /// Gets or sets the unique identifier of the refresh token.
        /// </summary>
        [Key]
        [Comment("Refresh token identifier.")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the token string.
        /// </summary>
        [Required]
        [Comment("The token string.")]
        public required string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the token.
        /// </summary>
        [Required]
        [Comment("The expiration date and time of the token.")]
        public DateTime Expires { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the token.
        /// </summary>
        [Required]
        [Comment("The date and time when the token was created.")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the revocation date of the token, if any.
        /// </summary>
        [Comment("The date and time when the token was revoked, if any.")]
        public DateTime? Revoked { get; set; }

        /// <summary>
        /// Gets a value indicating whether the token is currently active.
        /// </summary>
        [NotMapped]
        [Comment("Indicates whether the token is active.")]
        public bool IsActive => Revoked == null && DateTime.UtcNow < Expires;

        /// <summary>
        /// Gets or sets the identifier of the user to whom the token belongs.
        /// </summary>
        [Required]
        [Comment("Identifier of the user the token belongs to.")]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user to whom the token belongs.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
