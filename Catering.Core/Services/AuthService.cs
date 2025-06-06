using Catering.Core.Constants;
using Catering.Core.Contracts;
using Catering.Core.DTOs.Identity;
using Catering.Core.Models.Email;
using Catering.Infrastructure.Common;
using Catering.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Catering.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IEmailService emailService;
        private readonly IRepository repository;

        public AuthService(UserManager<ApplicationUser> _userManager, IConfiguration _config, IEmailService _emailService, IRepository _repository)
        {
            userManager = _userManager;
            config = _config;
            emailService = _emailService;
            repository = _repository;
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

            int expiresInMinutes = int.TryParse(config["Jwt:ExpiresInMinutes"], out var parsed) ? parsed : 15;

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

            if (!identityUser.EmailConfirmed)
            {
                await SendConfirmationEmail(identityUser, user.ClientUri);
                throw new UnauthorizedAccessException("Email not confirmed. A new confirmation email has been sent to your address.");
            }

            bool passwordValid = await userManager.CheckPasswordAsync(identityUser, user.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var userRoles = await userManager.GetRolesAsync(identityUser);
            string token = await GenerateTokenString(identityUser, userRoles);

            string refreshToken = await CreateRefreshToken(identityUser.Id);

            LoginResponseDto response = new LoginResponseDto()
            {
                UserId = identityUser.Id,
                Username = identityUser.UserName ?? "Error",
                Email = identityUser.Email ?? "Error",
                Token = token,
                RefreshToken = refreshToken,
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

            List<string> roles = [RoleNames.User];

            List<Restaurant> restaurants = await repository
                .All<Restaurant>()
                .Where(r => r.ContactEmail == user.Email && r.OwnerId == null)
                .ToListAsync();

            foreach (var restaurant in restaurants)
            {
                restaurant.OwnerId = identityUser.Id;
            }

            if (restaurants.Count > 0)
            {
                await repository.SaveChangesAsync();
                roles.Add(RoleNames.RestaurantOwner);
            }

            await userManager.AddToRolesAsync(identityUser, roles);

            await SendConfirmationEmail(identityUser, user.ClientUri);

            return result;
        }

        public async Task Logout(LogoutRequestDto logoutRequest, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var refreshToken = await repository
                .All<RefreshToken>()
                .FirstOrDefaultAsync(rt => rt.Token == logoutRequest.RefreshToken && rt.UserId == user.Id);

            if (refreshToken == null)
            {
                throw new InvalidOperationException("Refresh token not found.");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            await repository.SaveChangesAsync();
        }

        public async Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto refreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);

            var username = principal.Identity?.Name;
            if (username == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var identityUser = await userManager.FindByNameAsync(username);
            if (identityUser == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var refreshToken = await repository
                .All<RefreshToken>()
                .Where(rt => rt.Token == refreshTokenDto.RefreshToken && rt.UserId == identityUser.Id)
                .FirstOrDefaultAsync();

            if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.Revoked != null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            await repository.SaveChangesAsync();

            var userRoles = await userManager.GetRolesAsync(identityUser);
            string newJwtToken = await GenerateTokenString(identityUser, userRoles);
            string newRefreshToken = await CreateRefreshToken(identityUser.Id);

            return new RefreshTokenResponseDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
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

            var decodedToken = WebUtility.UrlDecode(user.Token);

            var result = await userManager.ResetPasswordAsync(identityUser, decodedToken, user.Password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Reset password failed: {errorMessages}");
            }

            await RevokeAllRefreshTokens(identityUser.Id);
        }

        public async Task EmailConfirmation(string email, string token)
        {
            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                throw new UnauthorizedAccessException("User does not exist");
            }

            var decodedToken = WebUtility.UrlDecode(token);

            var confirmResult = await userManager.ConfirmEmailAsync(identityUser, decodedToken);

            if (!confirmResult.Succeeded)
            {
                throw new UnauthorizedAccessException($"Invalid email confirmation request");
            }
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> CreateRefreshToken(string userId)
        {
            var refreshToken = GenerateRefreshTokenString();

            await repository.AddAsync(new RefreshToken()
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = userId,
            });

            await repository.SaveChangesAsync();

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = config.GetSection("Jwt");
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Key is not configured in the application settings."))),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (jwtSecurityToken.ValidTo > DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token is not expired yet");
            }

            return principal;
        }

        private async Task RevokeAllRefreshTokens(string userId)
        {
            var refreshTokens = await repository
                .All<RefreshToken>()
                .Where(rt => rt.UserId == userId && rt.Revoked == null && rt.Expires > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in refreshTokens)
            {
                token.Revoked = DateTime.UtcNow;
            }

            if (refreshTokens.Any())
            {
                await repository.SaveChangesAsync();
            }
        }

        private async Task SendConfirmationEmail(ApplicationUser user, string clientUri)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException("Cannot send confirmation email: user's email is missing.");
            }

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", confirmationToken },
                { "email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var message = new Message([user.Email], "Email Confirmation Token", callback);
            await emailService.SendEmailAsync(message);
        }

    }
}
