using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.DTOs;
using ChallengeAtmApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAtmApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        public OperationsController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        /*
         Endpoint Operaciones: La API debe contar con un endpoint que, dado un número de
tarjeta, retorne el historial de todas las operaciones realizadas. La respuesta debe estar
paginada (páginas de 10 registros
        */
        [HttpGet("{cardNumber}")]
        [Authorize]
        public async Task<ActionResult<TransactionOperationsDto>> CheckOperations(int cardNumber, [FromQuery] int page = 1)
        {
            try
            {
                var operationsResponse = await _transactionHistoryService.DoCheckOpertaionsAsync(cardNumber, page);
                if (operationsResponse != null)
                {
                    return Ok(operationsResponse);
                }
                else
                {
                    return BadRequest("No operations where found for your card number");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: ", ex);
            }
        }
    }
}
