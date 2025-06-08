using Catering.Core.DTOs.Partnership;

namespace Catering.Core.Contracts
{
    public interface IPartnershipService
    {
        Task<int> SubmitRequestAsync(PartnershipDto dto);
        Task ProcessRequestAsync(ManagePartnershipDto manageRequestDto);
        Task<PartnershipRequestListDto> GetAllPartnershipRequestsAsync(PartnershipRequestQueryParametersDto queryParams);
    }
}
