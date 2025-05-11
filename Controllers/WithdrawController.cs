using ChallengeAtmApi.Applications.Services.Interfaces;
using ChallengeAtmApi.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        public WithdrawController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> WithdrawFromAccount([FromBody] TransactionWithdrawRequest body)
        {
            try
            {
                var transaction = await _transactionHistoryService.DoTransactionWithdrawAsync(body.cardNumber, body.amount);
                if (transaction != null)
                {
                    var transactionResume = new
                    {
                        body.cardNumber,
                        amountToWithdraw = body.amount,
                        remainingBalance = transaction.RemainingBalance,
                        customerId = transaction.Id
                    };
                    return Ok(new { success = "La transacción se realizó con exito", resume = transactionResume });
                }
                else
                {
                    return BadRequest("Account not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: ", ex);
            }
        }
    }
}
