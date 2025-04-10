using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Application.Services
{
    public class CardInformationService : ICardInformationService
    {
        private readonly PostgresContext _context;
        public CardInformationService(PostgresContext context)
        {
            _context = context;

        }
        public async Task<CardInformation> SetCardBlockedState(int cardNumber)
        {
            try
            {
                var CardDetails = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
                if (CardDetails != null)
                {
                    CardDetails.IsBlocked = true;
                    _context.CardInformations.Update(CardDetails);
                    await _context.SaveChangesAsync();
                    return CardDetails;
                }
                else
                {
                    throw new Exception("No se encuentra la tarjeta solicitada");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado:", ex);
            }
        }

        public async Task<CardInformation> GetCardInformationAsync(int cardNumber)
        {
            try
            {
                var cardInfo = await _context.CardInformations.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
                if (cardInfo != null)
                {
                    return cardInfo;
                }
                throw new Exception("Card not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching card info: ", ex);
            }
        }

    }
}
