using System.ComponentModel.DataAnnotations;
using static Catering.Core.Constants.ErrorMessageConstants;

namespace Catering.Core.DTOs.Partnership
{
    public class ManagePartnershipDto
    {
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = RequiredMessage)]
        public int PartnershipRequestId { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        public bool isApproved { get; set; }
    }
}
