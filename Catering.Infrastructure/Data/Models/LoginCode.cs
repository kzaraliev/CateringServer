using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.LoginCodeConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a one-time login code (OTP) generated for passwordless authentication.
    /// </summary>
    [Comment("Represents a one-time login code (OTP) generated for passwordless authentication.")]
    public class LoginCode
    {
        /// <summary>
        /// Gets or sets the unique identifier for the login code.
        /// </summary>
        [Key]
        [Comment("Unique identifier for the login code.")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the actual numeric or alphanumeric code sent to the user.
        /// </summary>
        [Required]
        [MaxLength(CodeMaxLength)]
        [Comment("The actual numeric or alphanumeric code sent to the user.")]
        public required string Code { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the login code was created.
        /// </summary>
        [Required]
        [Comment("UTC date and time when the login code was created.")]
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the login code expires.
        /// </summary>
        [Required]
        [Comment("UTC date and time when the login code expires.")]
        public required DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the login code was used.
        /// Null if the code has not yet been used.
        /// </summary>
        [Comment("UTC date and time when the login code was used. Null if not yet used.")]
        public DateTime? UsedAt { get; set; }

        /// <summary>
        /// Gets a value indicating whether the login code has been used.
        /// </summary>
        /// <remarks>
        /// This is a computed property based on whether <see cref="UsedAt"/> has a value.
        /// </remarks>
        [NotMapped]
        [Comment("Indicates whether the login code has been used.")]
        public bool IsUsed => UsedAt.HasValue;

        /// <summary>
        /// Gets or sets the ID of the user for whom this login code was generated.
        /// </summary>
        [Required]
        [Comment("ID of the user for whom this login code was generated.")]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ApplicationUser"/> associated with this login code.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        [Comment("The ApplicationUser associated with this login code.")]
        public ApplicationUser User { get; set; } = null!;
    }
}
