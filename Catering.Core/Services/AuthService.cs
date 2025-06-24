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

        public async Task<IdentityResult> RegisterAsync(RegisterRequestDto user)
        {
            var identityUser = new ApplicationUser()
            {
                Email = user.Email,
                UserName = user.Username,
            };

            var result = await userManager.CreateAsync(identityUser);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Registration failed: {errorMessages}");
            }

            List<string> roles = [RoleNames.User];

            List<Restaurant> restaurants = await repository
                .AllReadOnly<Restaurant>()
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

            await SendConfirmationEmailAsync(identityUser, user.ClientUri);

            return result;
        }

        public async Task LogoutAsync(LogoutRequestDto logoutRequest, string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var user = await userManager.FindByIdAsync(userId);

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

        public async Task RequestLoginCodeAsync(RequestLoginCodeDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return;
            }

            if (!user.EmailConfirmed)
            {
                await SendConfirmationEmailAsync(user, request.ClientUri);
                throw new UnauthorizedAccessException("Email not confirmed. A new confirmation email has been sent to your address.");
            }

            var existingCodes = await repository
                .All<LoginCode>()
                .Where(lc => lc.UserId == user.Id && lc.UsedAt == null && lc.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var code in existingCodes)
            {
                code.UsedAt = DateTime.UtcNow;
            }

            if (existingCodes.Any())
            {
                await repository.SaveChangesAsync();
            }

            string sixDigitCode = GenerateNumericCode(6);
            int expiresInMinutes = int.Parse(config["Auth:LoginCodeExpiresInMinutes"] ?? "10");

            var loginCode = new LoginCode
            {
                UserId = user.Id,
                Code = sixDigitCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
            };

            await repository.AddAsync(loginCode);
            await repository.SaveChangesAsync();

            var emailSubject = "Your Login Code";
            var emailBody = $"Your one-time login code is: <strong>{sixDigitCode}</strong>. This code is valid for {expiresInMinutes} minutes. If you did not request this, please ignore this email.";
            await emailService.SendEmailAsync(new Message([request.Email], emailSubject, emailBody));
        }

        public async Task<LoginResponseDto> VerifyLoginCodeAsync(VerifyLoginCodeDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var loginCode = await repository
                .All<LoginCode>()
                .Where(lc => lc.UserId == identityUser.Id && lc.Code == request.Code && lc.UsedAt == null)
                .OrderByDescending(lc => lc.CreatedAt)
                .FirstOrDefaultAsync();

            if (loginCode == null || loginCode.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired login code.");
            }

            loginCode.UsedAt = DateTime.UtcNow;
            await repository.SaveChangesAsync();

            await RevokeAllRefreshTokensAsync(identityUser.Id);

            var userRoles = await userManager.GetRolesAsync(identityUser);
            string jwtToken = await GenerateTokenStringAsync(identityUser, userRoles);
            string refreshToken = await CreateRefreshTokenAsync(identityUser.Id);

            var response = new LoginResponseDto
            {
                UserId = identityUser.Id,
                Username = identityUser.UserName ?? null,
                Email = identityUser.Email ?? "Error",
                Token = jwtToken,
                RefreshToken = refreshToken,
                Roles = userRoles
            };

            return response;
        }

        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto, string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var identityUser = await userManager.FindByIdAsync(userId);
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
                throw new ArgumentException("The provided refresh token is invalid, expired, or has been revoked.");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            await repository.SaveChangesAsync();

            var userRoles = await userManager.GetRolesAsync(identityUser);
            string newJwtToken = await GenerateTokenStringAsync(identityUser, userRoles);
            string newRefreshToken = await CreateRefreshTokenAsync(identityUser.Id);

            return new RefreshTokenResponseDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto user)
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

        public async Task ResetPasswordAsync(ResetPasswordRequestDto user)
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

            await RevokeAllRefreshTokensAsync(identityUser.Id);
        }

        public async Task EmailConfirmationAsync(string email, string token)
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

        private async Task<string> CreateRefreshTokenAsync(string userId)
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

        private async Task RevokeAllRefreshTokensAsync(string userId)
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

        private async Task SendConfirmationEmailAsync(ApplicationUser user, string clientUri)
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

        private Task<string> GenerateTokenStringAsync(IdentityUser user, IEnumerable<string> roles)
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

        private string GenerateNumericCode(int length)
        {
            var random = new Random();
            string code = string.Empty;
            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 9).ToString();
            }
            return code;
        }
    }
}
