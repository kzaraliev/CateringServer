using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a specific day within a restaurant's working time schedule.
    /// </summary>
    [Comment("Represents a specific day within a restaurant's working time schedule.")]
    public class WorkingDay
    {
        /// <summary>
        /// Gets or sets the unique identifier for the working day.
        /// </summary>
        [Key]
        [Comment("Working Day Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the day of the week.
        /// </summary>
        [Required]
        [Comment("Day of the week.")]
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// Gets or sets the opening time for the day.
        /// </summary>
        [Required]
        [Comment("Opening time for the day.")]
        public TimeSpan OpenTime { get; set; }

        /// <summary>
        /// Gets or sets the closing time for the day.
        /// </summary>
        [Required]
        [Comment("Closing time for the day.")]
        public TimeSpan CloseTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the restaurant is closed on this day.
        /// </summary>
        [Comment("Indicates whether the restaurant is closed on this day.")]
        public bool IsClosed { get; set; } = false;

        /// <summary>
        /// Gets or sets the foreign key to the related working time schedule.
        /// </summary>
        [Required]
        [Comment("Foreign key to the related WorkingTime schedule.")]
        public int WorkingTimeId { get; set; }

        /// <summary>
        /// Gets or sets the related working time schedule.
        /// </summary>
        [ForeignKey(nameof(WorkingTimeId))]
        public WorkingTime WorkingTime { get; set; } = null!;
    }
}
