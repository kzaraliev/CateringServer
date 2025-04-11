using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents the overall working time schedule for a restaurant.
    /// </summary>
    [Comment("Represents the overall working time schedule for a restaurant.")]
    public class WorkingTime
    {
        /// <summary>
        /// Gets or sets the unique identifier for the working time schedule.
        /// </summary>
        [Key]
        [Comment("Working Time Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Collection of working days that define the schedule.
        /// </summary>
        [Comment("Collection of working days for the schedule")]
        public ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
    }
}
