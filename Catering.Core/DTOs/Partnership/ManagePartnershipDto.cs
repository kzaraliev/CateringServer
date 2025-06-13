using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Partnership
{
    public class ManagePartnershipDto
    {
        [Required(ErrorMessage = RequiredMessage)]
        public bool isApproved { get; set; }
    }
}
