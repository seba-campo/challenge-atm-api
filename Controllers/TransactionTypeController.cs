using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengeAtmApi.Controllers
{
    [Route("api/transaction-type")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypeController(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypes()
        {
            // Obtén todos los registros de la tabla "Auth"
            var auths = await _transactionTypeService.GetAllTransactionType();
            return Ok(auths);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypeById(Guid id)
        {
            var transactionType = await _transactionTypeService.GetTransactionTypeById(id);
            if(transactionType == null)
            {
                return NotFound(id);
            }
            return Ok(transactionType);
        }
    }
}
