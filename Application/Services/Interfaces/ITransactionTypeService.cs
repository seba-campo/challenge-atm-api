using ChallengeAtmApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface ITransactionTypeService
    {
        Task<IEnumerable<TransactionType>> GetAllTransactionType();
        Task<TransactionType?> GetTransactionTypeById(Guid id);
        Task<TransactionType?> GetTransactionTypeByDescription(string query);
        Task<string> GetTransactionDescriptionById(Guid id);
    }
}
