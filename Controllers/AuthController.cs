using ChallengeAtmApi.Applications.Services.Interfaces;
using ChallengeAtmApi.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengeAtmApi.Api.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // POST: api/<AuthController>
        [HttpPost]
        public async Task<ActionResult<bool>> Login([FromBody] GetAuthDto request)
        {
            if (await _authService.AuthCardAndPin(request.cardNumber, request.pin))
            {
                var token = await _authService.LogInUser(request.cardNumber);
                return Ok(new { acessToken = token });
            }
            return Unauthorized("Invalid credentials or card blocked.");
        }
    }
}
