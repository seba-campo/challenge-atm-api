using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Domain.Models;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Application.Services
{
    public class FailedLoginAttemptService : IFailedLoginAttemptService
    {
        private readonly PostgresContext _context;
        private readonly ICardInformationService _cardInformationService;
        public FailedLoginAttemptService(PostgresContext context, ICardInformationService cardInformationService)
        {
            _context = context;
            _cardInformationService = cardInformationService;
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

                if (failedLoginAttempt.AttemptCount > 3)
                {
                    await _cardInformationService.SetCardBlockedState(card);
                }

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
