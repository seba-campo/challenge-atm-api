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
    }
}
