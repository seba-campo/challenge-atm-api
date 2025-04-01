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
        public AuthService(PostgresContext context)
        {
            _context = context;
        }
        public async Task<Boolean> AuthCardAndPin(int card, string pin)
        {
            var auth = await GetAuthByCardNumber(card);
            if (auth != null ) {
                if(auth.CardNumber == card && auth.HashedPin == pin)
                {
                    //Buscar si está bloqueada la tarjeta. 
                    if (await IsCardBlocked(card))
                    {
                        //Sumar un intento a la tabla correspondiente
                        await AddLoginAttempt(card);
                        return false;
                    }
                    else
                    {
                        //Generar token, y 
                        var message = "test informacion";
                        var messageEncrypted = AesEncryption.Encrypt(message);
                        var messageDecrypted = AesEncryption.Decrypt(messageEncrypted);

                        Console.WriteLine("Mensaje original: {0}, Encriptado: {1}, Desencriptado: {2}", message, messageEncrypted, messageDecrypted);
                        //Cargar token en la tabla correspondiente.
                        return true;
                    }
                };
            };
            return false;
        }
        public async Task<Auth?> GetAuthByCardNumber(int cardNumber)
        {
            //return await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber
            var auth = await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            return auth;
        }
        public async Task<Boolean> IsCardBlocked(int cardNumber)
        {
            var cardInfo = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
            if (cardInfo.IsBlocked) { return true; }
            else { return false; }
        }
        public async Task<FailedLoginAttempt> AddLoginAttempt(int card)
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
