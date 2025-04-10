using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Application.Services
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly PostgresContext _context;
        public TransactionTypeService(PostgresContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TransactionType>> GetAllTransactionType()
        {
            // Obtén todos los registros de la tabla "Auth"
            var transactionTypes = await _context.TransactionTypes.ToListAsync();
            return transactionTypes;
        }
        public async Task<TransactionType?> GetTransactionTypeById(Guid id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }

        public async Task<TransactionType?> GetTransactionTypeByDescription(string query)
        {
            try
            {
                var transacitonType = await _context.TransactionTypes.FirstOrDefaultAsync(t => t.Description == query);
                if (transacitonType != null)
                {
                    return transacitonType;
                }
                throw new Exception("The transaction type you tiped does not exist");
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching the desired transaction type: ", ex);
            }
        }
        public async Task<string> GetTransactionDescriptionById(Guid id)
        {
            try
            {
                var transactionType = await _context.TransactionTypes.FindAsync(id);
                if(transactionType != null)
                {
                    return transactionType.Description;
                }
                else
                {
                    throw new Exception("Not existing transaction type with this Id");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error while fetching the desired transaction type: ", ex);
            }
        }

    }
}
