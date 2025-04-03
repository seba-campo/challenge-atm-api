using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Boolean> AuthCardAndPin(int card, string pin);
        Task<string?> LogInUser(int cardNumber);
    }
}
