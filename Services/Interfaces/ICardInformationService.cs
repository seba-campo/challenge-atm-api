using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ICardInformationService
    {
        Task<CardInformation> SetCardBlockedState(int cardNumber);
        Task<CardInformation> GetCardInformationAsync(int cardNumber);
    }
}
