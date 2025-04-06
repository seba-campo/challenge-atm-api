using ChallengeAtmApi.Context;
using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly PostgresContext _context;
        private readonly ICustomerInformationService _customerInformationService;
        private readonly ITransactionTypeService _transactionTypeService;
        public TransactionHistoryService (PostgresContext context, ICustomerInformationService customerInformationService, ITransactionTypeService transactionTypeService)
        {
            _context = context;
            _customerInformationService = customerInformationService;
            _transactionTypeService = transactionTypeService;
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
                var lastTransaction = await _context.TransactionHistories
                    .Where(t => t.CustomerId == customerInformation.Id)
                    .OrderByDescending(t => t.TransactionDateTime)
                    .FirstOrDefaultAsync();
                
                
                var data = new TransactionCheckDto();
                data.nombre = customerInformation.UserName;
                data.accountId = customerInformation.Id;
                data.balance = customerInformation.AccountBalance;
                data.lastTransaction = lastTransaction;
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        //Task<TransactionHistory> DoTransactionTransferAsync(Guid customerId, float ammout, Guid destinationCustomerId);
        public async Task<TransactionHistory> DoTransactionDepositAsync(Guid customerId, double amount)
        {
            try
            {
                var operation = await _customerInformationService.AddAmmountToBalance(customerId, amount);
                if (operation != null)
                {
                    Console.WriteLine("Deposito hecho");
                    var transactionType = await _transactionTypeService.GetTransactionTypeByDescription("deposit");
                    var transaction = new TransactionHistory
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerId,
                        TransactionTypeId = transactionType.Id,
                        TransactionAmount = amount,
                        RemainingBalance = operation.AccountBalance,
                        TransactionDateTime = DateOnly.FromDateTime(DateTime.UtcNow),
                    };
                    var newTransactionHistory = await _context.TransactionHistories.AddAsync(transaction);
                    await _context.SaveChangesAsync();
                    return transaction;
                }
                else
                {
                    throw new Exception("The account doesn't exist");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error while proccessing transaction: ", ex);
            }
        }

    }
}
