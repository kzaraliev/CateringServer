using Catering.Core.DTOs.Identity;
using Microsoft.AspNetCore.Identity;

namespace Catering.Core.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto user);
        Task<IdentityResult> Register(RegisterRequestDto user);
        Task ForgotPassword(ForgotPasswordRequestDto user);
        Task ResetPassword(ResetPasswordRequestDto user);
        Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto refreshTokenDto);
        Task Logout(LogoutRequestDto logoutRequest, string username);
        Task EmailConfirmation(string email, string token);
    }
}
