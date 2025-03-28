using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
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
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetAllTransactionType()
        {
            // Obtén todos los registros de la tabla "Auth"
            return await _context.TransactionTypes.ToListAsync();
        }
        public async Task<TransactionType?> GetTransactionTypeById(Guid id)
        {
            return await _context.TransactionTypes.FindAsync(id);
        }
    }
}
