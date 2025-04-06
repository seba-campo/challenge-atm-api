using ChallengeAtmApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionType>> GetAllTransactionType();
        Task<TransactionType?> GetTransactionTypeById(Guid id);
        Task<TransactionType?> GetTransactionTypeByDescription(string query);
    }
}
