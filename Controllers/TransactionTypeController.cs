using ChallengeAtmApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengeAtmApi.Controllers
{
    [Route("api/transaction-type")]
    [ApiController]
    public class TransactionTypeController(PostgresContext context) : ControllerBase
    {
        private readonly PostgresContext _context = context;

        // GET: api/Auth
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactionTypes()
        {
            // Obtén todos los registros de la tabla "Auth"
            var auths = await _context.TransactionTypes.ToListAsync();
            return Ok(auths);
        }
    }
}
