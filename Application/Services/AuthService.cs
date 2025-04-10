using System.Linq.Expressions;
using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using ChallengeAtmApi.Infrastructure.Context;
using ChallengeAtmApi.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly PostgresContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFailedLoginAttemptService _failedLoginAttemptService;
        private Guid authId;
        public AuthService(PostgresContext context, ITokenService tokenService, IFailedLoginAttemptService failedLoginAttemptService)
        {
            _context = context;
            _tokenService = tokenService;
            _failedLoginAttemptService = failedLoginAttemptService;
        }
        public async Task<bool> AuthCardAndPin(int card, string pin)
        //Valido existencia y coincidencia entre card y pin.
        {
            var auth = await GetAuthByCardNumber(card);
            var pinHashed = AesEncryption.Encrypt(pin);
            if (auth != null)
            {
                if (auth.CardNumber == card && auth.HashedPin == pinHashed)
                {
                    //Buscar si está bloqueada la tarjeta. 
                    if (await IsCardBlocked(card))
                    {

                        return false;
                    }
                    else
                    {
                        authId = auth.Id;
                        await _failedLoginAttemptService.ResetLoginAttempt(card);
                        return true;
                    }
                }
                else
                {
                    await _failedLoginAttemptService.AddLoginAttempt(card);
                    return false;
                }
            };
            return false;
        }
        public async Task<string?> LogInUser(int cardNumber)
        {
            try
            {
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
            try
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
            catch (Exception ex)
            {
                throw new Exception("Card not found: ", ex);
            }
        }
        private async Task<Auth?> GetAuthByCardNumber(int cardNumber)
        {
            //return await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber
            try
            {
                var auth = await _context.Auths.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
                return auth;
            }
            catch (Exception ex)
            {
                throw new Exception("Card not found: ", ex);
            }
        }
        private async Task<bool> IsCardBlocked(int cardNumber)
        {
            try
            {
                var cardInfo = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
                if (cardInfo.IsBlocked) { return true; }
                else { return false; }
            }
            catch (Exception ex)
            {
                throw new Exception("Card not found: ", ex);
            }
        }
    }
}
