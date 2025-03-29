using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;

namespace ChallengeAtmApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly PostgresContext _context;
        public AuthService(PostgresContext context)
        {
            _context = context;
        }
        public async Task<Boolean> AuthCardAndPin(int card, string pin)
        {
            var auth = await GetAuthByCardNumber(card);
            if (auth != null) {
                if(auth.CardNumber == card && auth.HashedPin == pin)
                {
                    return true;
                };
            };
            return false;
        }

        public async Task<Auth?> GetAuthByCardNumber(int cardNumber)
        {
            return await _context.Auths.FindAsync(cardNumber);
        }
    }
}
