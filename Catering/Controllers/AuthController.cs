using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.RegisterAsync(user);
            return StatusCode(201, new { message = "User registered successfully" });
        }

        [HttpPost("request-login-code")]
        [AllowAnonymous]    
        public async Task<IActionResult> RequestLoginCode(RequestLoginCodeDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await authService.RequestLoginCodeAsync(request);
            return Ok(new { message = "Login code is sent"});
        }

        [HttpPost("verify-login-code")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyLoginCode(VerifyLoginCodeDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.VerifyLoginCodeAsync(request);
            return Ok(result);
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await authService.RefreshTokenAsync(refreshTokenRequest, userId);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto logoutRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await authService.LogoutAsync(logoutRequest, userId);
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
