using ChallengeAtmApi.Core.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username);
        Task<Token?> SaveTokenAsync(string token, Guid authId);
    }
}
