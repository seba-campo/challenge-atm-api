using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengeAtmApi.API.Controllers
{
    [Route("api/Transaction-type")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypeController(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypes()
        {
            // Obtén todos los registros de la tabla "Auth"
            var auths = await _transactionTypeService.GetAllTransactionType();
            return Ok(auths);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypeById(Guid id)
        {
            var transactionType = await _transactionTypeService.GetTransactionTypeById(id);
            if (transactionType == null)
            {
                return NotFound(id);
            }
            return Ok(transactionType);
        }
    }
}
