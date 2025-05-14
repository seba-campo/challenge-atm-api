using ChallengeAtmApi.Domain.Models;

namespace ChallengeAtmApi.Application.Services.Interfaces
{
    public interface IFailedLoginAttemptService
    {
        Task<FailedLoginAttempt> AddLoginAttempt(int card);
        Task<FailedLoginAttempt> ResetLoginAttempt(int card);
    }
}
