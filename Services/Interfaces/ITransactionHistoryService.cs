using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<TransactionHistory> DoTransactionWithdrawAsync(int cardNumber, float amount);
        Task<TransactionCheckDto> DoTransactionCheckAsync(int cardNumber);
        Task<TransactionHistory> DoTransactionDepositAsync(Guid customerId, double ammount);
    }
}
