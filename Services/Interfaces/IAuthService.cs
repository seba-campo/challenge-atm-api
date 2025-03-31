using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Boolean> AuthCardAndPin(int card, string pin);

        Task<Auth?> GetAuthByCardNumber(int card);
        Task<Boolean> IsCardBlocked(int cardNumber);
    }
}
