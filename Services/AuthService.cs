using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Security;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly PostgresContext _context;
        private readonly ITokenService _tokenService;
        private Guid authId;
        public AuthService(PostgresContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<Boolean> AuthCardAndPin(int card, string pin)
            //Valido existencia y coincidencia entre card y pin.
        {
            var auth = await GetAuthByCardNumber(card);
            var pinHashed = AesEncryption.Encrypt(pin);
            if (auth != null ) {
                if(auth.CardNumber == card && auth.HashedPin == pinHashed)
                {
                    //Buscar si está bloqueada la tarjeta. 
                    if (await IsCardBlocked(card))
                    { 
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    await AddLoginAttempt(card);
                    return false;
                }
            };
            return false;
        }
        public async Task<string?> LogInUser(int cardNumber)
        {
            try {
                var userName = await GetCustomerNameAsync(cardNumber);
                if (userName != null)
                {
                    //Generar token, guardarlo y retornarlo
                    var token = _tokenService.GenerateJwtToken(userName);
                    await _tokenService.SaveTokenAsync(token, authId);
                    return token.ToString();
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado", ex);
            }
            
        }
        private async Task<string?> GetCustomerNameAsync(int cardNumber)
        {
            var cardInfo = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            if (cardInfo != null)
            {
                var customerInfo = await _context.CustomerInformations.FirstOrDefaultAsync(c => c.Id == cardInfo.CustomerId);
                return customerInfo.UserName;
            }
            else
            {
                return null;
            }
        }
        private async Task<Auth?> GetAuthByCardNumber(int cardNumber)
        {
            //return await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber
            var auth = await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            return auth;
        }
        private async Task<Boolean> IsCardBlocked(int cardNumber)
        {
            var cardInfo = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            if (cardInfo.IsBlocked) { return true; }
            else { return false; }
        }
        private async Task<FailedLoginAttempt> AddLoginAttempt(int card)
        {
            var failedLoginAttempt = await _context.FailedLoginAttempts.FirstOrDefaultAsync(f => f.CardNumber == card);
            if (failedLoginAttempt == null) {
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
    }
}
