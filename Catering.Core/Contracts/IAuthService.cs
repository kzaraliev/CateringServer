using Catering.Core.DTOs.Identity;
using Microsoft.AspNetCore.Identity;

namespace Catering.Core.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto user);
        Task<IdentityResult> RegisterAsync(RegisterRequestDto user);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto user);
        Task ResetPasswordAsync(ResetPasswordRequestDto user);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto);
        Task LogoutAsync(LogoutRequestDto logoutRequest, string username);
        Task EmailConfirmationAsync(string email, string token);
    }
}
