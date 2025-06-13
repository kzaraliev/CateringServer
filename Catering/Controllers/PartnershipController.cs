using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Partnership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RoleNames.Moderator}, {RoleNames.Admin}")]
    public class PartnershipController : ControllerBase
    {
        private readonly IPartnershipService partnershipService;

        public PartnershipController(IPartnershipService _partnershipService)
        {
            partnershipService = _partnershipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPartnershipRequests([FromQuery] PartnershipRequestQueryParametersDto queryParams)
        {
            var requests = await partnershipService.GetAllPartnershipRequestsAsync(queryParams);
            return Ok(requests);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> ProcessRequest(int id, ManagePartnershipDto manageRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await partnershipService.ProcessRequestAsync(id, manageRequestDto);
            return NoContent();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitRequest([FromBody] PartnershipDto partnershipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int requestId = await partnershipService.SubmitRequestAsync(partnershipDto);
            return Ok(new { Message = "Request submitted successfully.", RequestId = requestId });
        }
    }
}
