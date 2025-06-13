using Catering.Core.DTOs.Partnership;
using Catering.Core.DTOs.Queries;

namespace Catering.Core.Contracts
{
    public interface IPartnershipService
    {
        Task<int> SubmitRequestAsync(PartnershipDto dto);
        Task ProcessRequestAsync(int partnershipRequestId, ManagePartnershipDto manageRequestDto);
        Task<PagedResult<PartnershipItemsDto>> GetAllPartnershipRequestsAsync(PartnershipRequestQueryParametersDto queryParams);
    }
}
