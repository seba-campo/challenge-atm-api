using ChallengeAtmApi.Domain.Models;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface ICardInformationService
    {
        Task<CardInformation> SetCardBlockedState(int cardNumber);
        Task<CardInformation> GetCardInformationAsync(int cardNumber);
    }
}
