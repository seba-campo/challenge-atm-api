using ChallengeAtmApi.Domain.Models;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface ICustomerInformationService
    {
        Task<CustomerInformation> GetCustomerByCardNumber(int cardNumber);
        Task<CustomerInformation> GetCustomerById(Guid id);
        Task<CustomerInformation> AddAmmountToAccountByCard(int cardNumber, double amount);
        Task<CustomerInformation> WithdrawFromAccount(int cardNumber, double amount);

    }
}
