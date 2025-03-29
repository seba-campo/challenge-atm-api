using ChallengeAtmApi.DTOs;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengeAtmApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // GET: api/<AuthController>
        [HttpPost]
        public async IActionResult Login([FromBody] GetAuthDto request)
        {
            return await _authService.AuthCardAndPin(request.cardNumber, request.hashedPin);
        }
        
    }
}
