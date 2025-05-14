using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Domain.DTOs;
using ChallengeAtmApi.Domain.Models;
using ChallengeAtmApi.Infrastructure.Context;
using ChallengeAtmApi.projects.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChallengeAtmApi.Application.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly PostgresContext _context;
        private readonly ICustomerInformationService _customerInformationService;
        private readonly ITransactionTypeService _transactionTypeService;
        private readonly int pageSize = 10;
        private string transactionDepositDescription;
        private string transactionCheckDescription;
        private string transactionTransferDescription;
        private string transactionWithdrawDescription;


        public TransactionHistoryService(PostgresContext context, ICustomerInformationService customerInformationService, ITransactionTypeService transactionTypeService)
        {
            _context = context;
            _customerInformationService = customerInformationService;
            _transactionTypeService = transactionTypeService;
        }
        private async Task GetTransactionsDescriptions()
        {
            // No me convence la performance de esta funcionalidad
            transactionDepositDescription = await _transactionTypeService.GetTransactionDescriptionById(new Guid("9c030e77-26f9-4ab0-9847-ef45d206267f"));
            transactionTransferDescription = await _transactionTypeService.GetTransactionDescriptionById(new Guid("5a495c95-b623-4efb-9144-0f6cb810b192"));
            transactionCheckDescription = await _transactionTypeService.GetTransactionDescriptionById(new Guid("50c75c0b-1a91-4d4c-a3e2-83736a8237a3"));
            transactionWithdrawDescription = await _transactionTypeService.GetTransactionDescriptionById(new Guid("31188ddd-410f-4bae-889f-93b7cd687512"));

        }
        public async Task<TransactionHistory> DoTransactionWithdrawAsync(int cardNumber, float amount)
        {
            try
            {
                var transaction = await _customerInformationService.WithdrawFromAccount(cardNumber, amount);
                if (transaction != null)
                {
                    var transactionLog = await SaveTransactionLog(transaction.Id, "withdraw", amount, transaction.AccountBalance, cardNumber);
                    return transactionLog;
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
        public async Task<TransactionCheckDto> DoTransactionCheckAsync(int cardNumber)
        {
            try
            {
                var customerInformation = await _customerInformationService.GetCustomerByCardNumber(cardNumber);
                //No valido si customerInformation es null ya que de eso se encarga el metodo
                //Se espera un respuesta diferente a las demás, no retorno el SaveTransactionLog()
                var lastTransaction = await _context.TransactionHistories
                    .Where(t => t.CustomerId == customerInformation.Id)
                    .OrderByDescending(t => t.TransactionDateTime)
                    .FirstOrDefaultAsync();
                var transactionData = new TransactionCheckDto
                {
                    nombre = customerInformation.UserName,
                    accountId = customerInformation.Id,
                    balance = customerInformation.AccountBalance,
                    lastTransaction = lastTransaction.TransactionDateTime
                };
                await SaveTransactionLog(customerInformation.Id, "check", 0, customerInformation.AccountBalance, cardNumber);
                return transactionData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while proccessing transaction: ", ex);
            }
        }
        public async Task<TransactionHistory> DoTransactionDepositAsync(double amount, int cardNumber)
        {
            try
            {
                var operation = await _customerInformationService.AddAmmountToAccountByCard(cardNumber, amount);
                if (operation != null)
                {
                    var customer = await _customerInformationService.GetCustomerByCardNumber(cardNumber);
                    var transactionLog = await SaveTransactionLog(customer.Id, "deposit", amount, operation.AccountBalance, cardNumber);
                    return transactionLog;
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
        private async Task<TransactionHistory> SaveTransactionLog(
            Guid customerId,
            string transactionTypeString,
            double amount,
            double remainingBalance,
            int cardNumber
        )
        {
            try
            {
                var transactionType = await _transactionTypeService.GetTransactionTypeByDescription(transactionTypeString);

                var transactionLog = new TransactionHistory
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    TransactionTypeId = transactionType.Id,
                    CardNumber = cardNumber,
                    TransactionAmount = amount,
                    RemainingBalance = remainingBalance,
                    TransactionDateTime = DateOnly.FromDateTime(DateTime.UtcNow),
                };
                await _context.TransactionHistories.AddAsync(transactionLog);
                await _context.SaveChangesAsync();
                return transactionLog;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving transaction log", ex);
            }
        }
        public async Task<TransactionOperationsDto> DoCheckOpertaionsAsync(int cardNumber, int page)
        {
            try
            {
                await GetTransactionsDescriptions();

                var totalOfOperations = await _context.TransactionHistories
                        .Where(o => o.CardNumber == cardNumber)
                        .CountAsync();

                var totalOfPages = (int)Math.Ceiling((double)totalOfOperations / pageSize);

                var resultsPaginated = await _context.TransactionHistories
                        .Where(o => o.CardNumber == cardNumber)  //Filtro por usuario ID
                        .OrderByDescending(o => o.TransactionDateTime)
                        .Skip((page - 1) * pageSize)  //Indica los necesarios a saltar, segun el tamaño de la page Gemini
                        .Take(pageSize)  //Tomo la cantidad indicada en los params
                        .ToListAsync();


                //Formateo los resultados para una mejor lectura
                var resultsFormatted = new List<object>();

                foreach (var r in resultsPaginated)
                {
                    //Mapeo los ID de transacciones con descripciones.4
                    
                    var transactionDesc = "";
                    switch (r.TransactionTypeId.ToString())
                    {
                        case "9c030e77-26f9-4ab0-9847-ef45d206267f":
                            transactionDesc = transactionDepositDescription;
                            break;
                        case "5a495c95-b623-4efb-9144-0f6cb810b192":
                            transactionDesc = transactionTransferDescription;
                            break;
                        case "50c75c0b-1a91-4d4c-a3e2-83736a8237a3":
                            transactionDesc = transactionCheckDescription;
                            break;
                        case "31188ddd-410f-4bae-889f-93b7cd687512":
                            transactionDesc = transactionWithdrawDescription;
                            break;
                    }
                    var _r = new
                    {
                        idOperation = r.Id,
                        customerId = r.CustomerId,
                        transactionType = transactionDesc,
                        transactionAmount = r.TransactionAmount,
                        accountRemainingBlance = r.RemainingBalance,
                        transactionDate = r.TransactionDateTime,
                        cardNumber = r.CardNumber
                    };
                    resultsFormatted.Add(_r);
                };

                var responseObject = new TransactionOperationsDto
                {
                    Operations = resultsFormatted,
                    Pagination = new PaginationDto
                    {
                        totalOperations = totalOfOperations,
                        totalPages = totalOfPages,
                        actualPage = page,
                        paginationSize = pageSize
                    }
                };

                return responseObject;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while proccessing transaction: ", ex);
            }
        }
    }
}
