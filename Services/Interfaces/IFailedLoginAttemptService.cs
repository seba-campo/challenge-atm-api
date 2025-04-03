using ChallengeAtmApi.Models;

namespace ChallengeAtmApi.Services.Interfaces
{
    public interface IFailedLoginAttemptService
    {
        Task<FailedLoginAttempt> AddLoginAttempt(int card);
        Task<FailedLoginAttempt> ResetLoginAttempt(int card);
    }
}
