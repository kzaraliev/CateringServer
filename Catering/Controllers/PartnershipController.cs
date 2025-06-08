using Catering.Core.Contracts;
using Catering.Core.DTOs.Partnership;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            try
            {
                var requests = await partnershipService.GetAllPartnershipRequestsAsync(queryParams);
                return Ok(requests);
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while processing your request");
            }
            
        }


        [HttpPost("ProcessRequest")]
        public async Task<IActionResult> ProcessRequest(ManagePartnershipDto manageRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await partnershipService.ProcessRequestAsync(manageRequestDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while processing your request");
            }
        }


        [HttpPost("SubmitRequest")]
        public async Task<IActionResult> SubmitRequest([FromBody] PartnershipDto partnershipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int requestId = await partnershipService.SubmitRequestAsync(partnershipDto);
                return Ok(new { Message = "Request submitted successfully.", RequestId = requestId });
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while processing your request");
            }
        }
    }
}
