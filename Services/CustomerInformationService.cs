using ChallengeAtmApi.Context;
using ChallengeAtmApi.DTOs;
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

        public async Task<CustomerInformation> GetCustomerById(Guid id)
        {
            try
            {
                var customerData = await _context.CustomerInformations.FindAsync(id);
                if (customerData != null)
                {
                    return customerData;
                }
                throw new Exception("Customer not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching customer: ", ex);
            }
        }

        public async Task<CustomerInformation> AddAmmountToBalance(Guid id, double amount)
        {
            try
            {
                var customer = await GetCustomerById(id);
                if (customer != null)
                {
                    customer.AccountBalance += amount;
                    _context.CustomerInformations.Update(customer);
                    await _context.SaveChangesAsync();
                    var transactionData = new TransactionDepositDto
                    {
                        account = customer.Id,
                        ammountOfDeposit = amount,
                        balance = customer.AccountBalance
                    };
                    return customer;
                }
                else
                {
                    throw new Exception("Customer not found");
                }
            } catch (Exception ex) {
                throw new Exception("Error while processing the operation", ex);
            }
        }
    }
}
