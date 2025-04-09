using ChallengeAtmApi.Models;
using System.Text.Json.Serialization;

namespace ChallengeAtmApi.DTOs
{
    public class TransactionOperationsDto
    {
        public List<TransactionHistory> Operations { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
