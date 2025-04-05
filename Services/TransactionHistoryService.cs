using ChallengeAtmApi.Context;
using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;

namespace ChallengeAtmApi.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly PostgresContext _context;
        private readonly ICardInformationService _cardInformationService;
        private readonly ICustomerInformationService _customerInformationService;
        public TransactionHistoryService (PostgresContext context, ICardInformationService cardInformationService, ICustomerInformationService customerInformationService)
        {
            _context = context;
            _cardInformationService = cardInformationService;
            _customerInformationService = customerInformationService;
        }
        //public async Task<TransactionHistory> DoTransactionWithdrawAsync(Guid customerId, float ammount)
        //{

        //};
        public async Task<TransactionCheckDto> DoTransactionCheckAsync(int cardNumber)
        {
            try
            {
                //var cardInformation = await _cardInformationService.GetCardInformationAsync(cardNumber);
                var customerInformation = await _customerInformationService.GetCustomerByCardNumber(cardNumber);
                var transactionFilter = await _context.TransactionHistories.All(t => t.CustomerId == customerInformation.Id);
                var data = new TransactionCheckDto();
                data.nombre = customerInformation.UserName;
                data.accountId = customerInformation.Id;
                data.balance = customerInformation.AccountBalance;
                data.lastTransaction = (ICollection<TransactionHistory>)customerInformation.TransactionHistories.LastOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching account status", ex);
            }
        };
        //Task<TransactionHistory> DoTransactionTransferAsync(Guid customerId, float ammout, Guid destinationCustomerId);
        //Task<TransactionHistory> DoTransactionDepositAsync(Guid customerId, float ammount)
        //{

        //};
    }
}
