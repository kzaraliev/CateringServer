using Catering.Core.Contracts;
using Catering.Core.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.Login(user);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, $"An error occurred while processing your request");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.Register(user);
                return StatusCode(201, new { Message = "User registered successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await authService.ForgotPassword(user);
                return Ok(new { message = "If the email exists, a reset link has been sent" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await authService.ResetPassword(user);
                return Ok(new { message = "Password was successfully reset" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.RefreshToken(refreshTokenRequest);
                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutRequestDto logoutRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized("Username not found in token");
            }

            try
            {
                await authService.Logout(logoutRequest, username);
                return Ok(new { message = "Logout successful" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred"});
            }
        }

        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(ConfirmEmailRequestDto confirmEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await authService.EmailConfirmation(confirmEmail.Email, confirmEmail.Token);
                return Ok(new { message = "Email was successfully confirmed" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
