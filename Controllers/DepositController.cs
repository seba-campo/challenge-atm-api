using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        public DepositController(ITransactionHistoryService transactionHistoryService) { 
            _transactionHistoryService = transactionHistoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DepositIntoAccount([FromBody] TransactionDepositRequest body)
        {
            try
            {
                var transaction = await _transactionHistoryService.DoTransactionDepositAsync(body.ammountOfDeposit, body.cardNumber);
                if (transaction != null)
                {
                    return Ok(new { success = "El deposito se realizó con exito" });
                }
                else
                {
                    return BadRequest("Account not found");
                }
            }
            catch (Exception ex) {
                throw new Exception("Something went wrong: ", ex);
            }
        }
    }
}
