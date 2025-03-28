using ChallengeAtmApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.Services
{
    public interface ITransactionTypeService
    {
        Task<ActionResult<IEnumerable<TransactionType>>> GetAllTransactionType();
        Task<TransactionType?> GetTransactionTypeById(Guid id);
    }
}
