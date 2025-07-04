﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        /// Gets or sets the collection of restaurants owned by the user.
        /// </summary>
        [Comment("Collection of restaurants owned by the user.")]
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        /// <summary>
        /// Gets or sets the collection of orders placed by the user.
        /// </summary>
        [Comment("Collection of orders placed by the user.")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
