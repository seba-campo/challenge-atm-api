using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.DTOs;
using ChallengeAtmApi.projects.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        public BalanceController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        /*
         Endpoint Saldo: La API debe contar con un endpoint que, dado un número de tarjeta,
retorne la siguiente información: nombre del usuario, número de cuenta, saldo actual y
fecha de la última extracción.
        */
        [HttpGet("{cardNumber}")]
        [Authorize]
        public async Task<ActionResult<TransactionCheckDto>> CheckSaldo(int cardNumber)
        {
            try
            {
                var balance = await _transactionHistoryService.DoTransactionCheckAsync(cardNumber);
                if (balance != null)
                {
                    return Ok(new { consultaDeSaldo = balance });
                }
                else
                {
                    return NotFound("Card not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: ", ex);
            }
        }
    }
}
