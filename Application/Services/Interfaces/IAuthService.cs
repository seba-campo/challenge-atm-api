using ChallengeAtmApi.Core.Models;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AuthCardAndPin(int card, string pin);
        Task<string?> LogInUser(int cardNumber);
    }
}
