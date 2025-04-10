using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using ChallengeAtmApi.Core.DTOs;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAtmApi.Application.Services
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
                    Console.WriteLine(customerData);
                    return customerData;
                }
                throw new Exception("Not customer found for this card.");
            }
            catch (Exception ex)
            {
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

        public async Task<CustomerInformation> AddAmmountToAccountByCard(int cardNumber, double amount)
        {
            try
            {
                var customer = await GetCustomerByCardNumber(cardNumber);
                if (customer != null)
                {
                    customer.AccountBalance += amount;
                    _context.CustomerInformations.Update(customer);
                    await _context.SaveChangesAsync();

                    return customer;
                }
                else
                {
                    throw new Exception("Customer not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the operation", ex);
            }
        }
        public async Task<CustomerInformation> WithdrawFromAccount(int cardNumber, double amount)
        {
            try
            {
                var customer = await GetCustomerByCardNumber(cardNumber);
                if (customer != null)
                {
                    if (amount <= customer.AccountBalance)
                    {
                        customer.AccountBalance -= amount;
                        _context.CustomerInformations.Update(customer);
                        await _context.SaveChangesAsync();

                        return customer;
                    }
                    else
                    {
                        throw new Exception("The amount of withdraw is greater than the actual balance of the account.");
                    }
                }
                else
                {
                    throw new Exception("Customer not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while processing the operation", ex);
            }
        }
    }
}
