using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ICustomerInformationService
    {
        Task<CustomerInformation> GetCustomerByCardNumber(int cardNumber);
    }
}
