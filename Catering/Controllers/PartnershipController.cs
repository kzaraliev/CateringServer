using Catering.Core.Contracts;
using Catering.Core.DTOs.Partnership;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnershipController : ControllerBase
    {
        private readonly IPartnershipService partnershipService;
        private readonly UserManager<ApplicationUser> userManager;

        public PartnershipController(IPartnershipService _partnershipService, UserManager<ApplicationUser> _userManager)
        {
            partnershipService = _partnershipService;
            userManager = _userManager;
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
