using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.WorkingDay
{
    public class WorkingDayDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        [EnumDataType(typeof(DayOfWeek))]
        public DayOfWeek Day { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public bool IsClosed { get; set; }
    }
}
