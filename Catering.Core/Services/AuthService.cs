using Catering.Core.Contracts;
using Catering.Core.DTOs.Identity;
using Catering.Core.Models.Email;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Catering.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IEmailService emailService;

        public AuthService(UserManager<ApplicationUser> _userManager, IConfiguration _config, IEmailService _emailService)
        {
            userManager = _userManager;
            config = _config;
            emailService = _emailService;
        }

        private Task<string> GenerateTokenString(IdentityUser user, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Key is not configured in the application settings."));

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.NameId, user.Id),
                new(ClaimTypes.Name, user.UserName ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            int expiresInMinutes = int.TryParse(config["Jwt:ExpiresInMinutes"], out var parsed) ? parsed : 60;

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Task.FromResult(tokenString);
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            bool passwordValid = await userManager.CheckPasswordAsync(identityUser, user.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var userRoles = await userManager.GetRolesAsync(identityUser);
            string token = await GenerateTokenString(identityUser, userRoles);

            LoginResponseDto response = new LoginResponseDto()
            {
                UserId = identityUser.Id,
                Username = identityUser.UserName ?? "Error",
                Email = identityUser.Email ?? "Error",
                Token = token,
                Roles = userRoles
            };

            return response;
        }

        public async Task<IdentityResult> Register(RegisterRequestDto user)
        {
            var identityUser = new ApplicationUser()
            {
                UserName = user.Username,
                Email = user.Email
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Registration failed: {errorMessages}");
            }

            return result;
        }

        public async Task ForgotPassword(ForgotPasswordRequestDto user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Email);
            if (identityUser != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(identityUser);
                var param = new Dictionary<string, string?>
                {
                    {"token", token},
                    {"email", user.Email}
                };

                var callback = QueryHelpers.AddQueryString(user.ClientUri, param);

                var message = new Message([user.Email], "Resset password token", callback);

                await emailService.SendEmailAsync(message);
            }
        }

        public async Task ResetPassword(ResetPasswordRequestDto user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return;
            }

            var result = await userManager.ResetPasswordAsync(identityUser, user.Token, user.Password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Reset password failed: {errorMessages}");
            }
        }
    }
}
