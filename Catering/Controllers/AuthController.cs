using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RoleNames.User)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.LoginAsync(user);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.RegisterAsync(user);
            return StatusCode(201, new { Message = "User registered successfully" });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authService.ForgotPasswordAsync(user);
            return Ok(new { message = "If the email exists, a reset link has been sent" });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authService.ResetPasswordAsync(user);
            return Ok(new { message = "Password was successfully reset" });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.RefreshTokenAsync(refreshTokenRequest);
            return Ok(result);
        }

        [HttpPost("logout")]
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

            await authService.LogoutAsync(logoutRequest, username);
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("email-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation(ConfirmEmailRequestDto confirmEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authService.EmailConfirmationAsync(confirmEmail.Email, confirmEmail.Token);
            return Ok(new { message = "Email was successfully confirmed" });
        }
    }
}
