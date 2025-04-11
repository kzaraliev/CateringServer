using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Catering.Infrastructure.Constants.ApplicationUserConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents an application user in the system.
    /// This user can be a restaurant owner or a regular customer.
    /// </summary>
    [Comment("Represents an application user in the system.")]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [Required]
        [MaxLength(FirstNameMaxLength)]
        [Comment("User's first name.")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Required]
        [MaxLength(LastNameMaxLength)]
        [Comment("User's last name.")]
        public required string LastName { get; set; }

        /// <summary>
        /// Gets or sets the collection of restaurants owned by the user.
        /// </summary>
        [Comment("Collection of restaurants owned by the user.")]
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        /// <summary>
        /// Gets or sets the collection of orders placed by the user.
        /// </summary>
        [Comment("Collection of orders placed by the user.")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Gets or sets the collection of addresses associated with the user.
        /// </summary>
        [Comment("Collection of addresses associated with the user.")]
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
