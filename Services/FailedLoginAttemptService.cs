using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Services
{
    public class FailedLoginAttemptService : IFailedLoginAttemptService
    {
        private readonly PostgresContext _context;
        public FailedLoginAttemptService(PostgresContext context)
        {
            _context = context;
        }
        public async Task<FailedLoginAttempt> AddLoginAttempt(int card)
        {
            var failedLoginAttempt = await _context.FailedLoginAttempts.FirstOrDefaultAsync(f => f.CardNumber == card);
            if (failedLoginAttempt == null)
            {
                failedLoginAttempt = new FailedLoginAttempt
                {
                    Id = Guid.NewGuid(),
                    CardNumber = card,
                    AttemptCount = 1,
                    LastAttempt = DateTime.UtcNow
                };
                _context.FailedLoginAttempts.Add(failedLoginAttempt);
            }
            else
            {
                failedLoginAttempt.LastAttempt = DateTime.UtcNow;
                failedLoginAttempt.AttemptCount++;
                _context.FailedLoginAttempts.Update(failedLoginAttempt);

            }
            await _context.SaveChangesAsync();
            return failedLoginAttempt;
        }
        public async Task<FailedLoginAttempt> ResetLoginAttempt(int card)
        {
            var failedLoginAttempt = await _context.FailedLoginAttempts.FirstOrDefaultAsync(f => f.CardNumber == card);
            if (failedLoginAttempt == null)
            {
                throw new Exception("Not found");
            }
            else
            {
                failedLoginAttempt.AttemptCount = 0;
                _context.FailedLoginAttempts.Update(failedLoginAttempt);
                await _context.SaveChangesAsync();
                return failedLoginAttempt;
            }
        }
    }
}
