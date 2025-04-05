using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Services
{
    public class CustomerInformationService : ICustomerInformationService
    {
        private readonly PostgresContext _context;
        public CustomerInformationService(PostgresContext context)
        {
            _context = context;
        }
        public async Task<CustomerInformation> GetCustomerByCardNumber(int cardNumber)
        {
            try
            {
                var customerData = await _context.CustomerInformations
                    .Include(c => c.CardInformations)
                    .FirstOrDefaultAsync(c => c.CardInformations.Any(card => card.CardNumber == cardNumber));
                if (customerData != null)
                {
                    return customerData;
                }
                throw new Exception("Not customer found for this card.");
            }
            catch (Exception ex) {
                throw new Exception("Error fetching customer: ", ex);
            }
        }
    }
}
