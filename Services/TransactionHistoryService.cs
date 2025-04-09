﻿using ChallengeAtmApi.Context;
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
        private readonly int pageSize = 10;
        public TransactionHistoryService(PostgresContext context, ICustomerInformationService customerInformationService, ITransactionTypeService transactionTypeService)
        {
            _context = context;
            _customerInformationService = customerInformationService;
            _transactionTypeService = transactionTypeService;
        }
        public async Task<TransactionHistory> DoTransactionWithdrawAsync(int cardNumber, float amount)
        {
            try
            {
                var transaction = await _customerInformationService.WithdrawFromAccount(cardNumber, amount);
                if (transaction != null)
                {
                    var transactionLog = await SaveTransactionLog(transaction.Id, "withdraw", amount, transaction.AccountBalance);
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
                    lastTransaction = lastTransaction
                };
                await SaveTransactionLog(customerInformation.Id, "check", 0, customerInformation.AccountBalance);
                return transactionData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while proccessing transaction: ", ex);
            }
        }
        public async Task<TransactionHistory> DoTransactionDepositAsync(Guid customerId, double amount)
        {
            try
            {
                var operation = await _customerInformationService.AddAmmountToAccount(customerId, amount);
                if (operation != null)
                {
                    var transactionLog = await SaveTransactionLog(customerId, "deposit", amount, operation.AccountBalance);
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
            double remainingBalance
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
                var customer = await _customerInformationService.GetCustomerByCardNumber(cardNumber);
                if (customer != null)
                {
                    var totalOfOperations = await _context.TransactionHistories
                            .Where(o => o.CustomerId == customer.Id)
                            .CountAsync();

                    var totalOfPages = (int)Math.Ceiling((double)totalOfOperations / pageSize);

                    var resultsPaginated = await _context.TransactionHistories
                            .Where(o => o.CustomerId == customer.Id)  //Filtro por usuario ID
                            .OrderByDescending(o => o.TransactionDateTime)
                            .Skip((page - 1) * pageSize)  //Indica los necesarios a saltar, segun el tamaño de la page Gemini
                            .Take(pageSize)  //Tomo la cantidad indicada en los params
                            .ToListAsync();

                    var responseObject = new TransactionOperationsDto
                    {
                        Operations = resultsPaginated,
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
                else
                {
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex) { 
                throw new Exception("Error while proccessing transaction: ", ex);
            } 
        }
    } 
}
