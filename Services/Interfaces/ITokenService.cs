using ChallengeAtmApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
        Task<Token?> SaveTokenAsync(string token, Guid authId);
    }
}
