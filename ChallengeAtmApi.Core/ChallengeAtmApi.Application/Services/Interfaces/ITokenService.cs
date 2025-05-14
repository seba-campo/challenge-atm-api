using ChallengeAtmApi.Domain.Models;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
        Task<Token?> SaveTokenAsync(string token, Guid authId);
    }
}
