using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Services
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

    }
}
