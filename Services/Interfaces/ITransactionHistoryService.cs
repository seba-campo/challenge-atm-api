using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ITransactionHistoryService
    {
        //Task<TransactionHistory> DoTransactionWithdrawAsync(Guid customerId, float ammount);
        Task<TransactionCheckDto> DoTransactionCheckAsync(int cardNumber);
        //Task<TransactionHistory> DoTransactionTransferAsync(Guid customerId, float ammout, Guid destinationCustomerId);
        Task<TransactionHistory> DoTransactionDepositAsync(Guid customerId, float ammount);
    }
}
