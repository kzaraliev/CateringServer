namespace Catering.Core.Contracts
{
    public interface IAuthService
    {
        Task<string> GenerateTokenString(string email);
    }
}
