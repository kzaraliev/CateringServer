namespace Catering.Core.DTOs.Identity
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public string? Username { get; set; }
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string RefreshToken {  get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
