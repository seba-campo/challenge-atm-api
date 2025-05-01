using ChallengeAtmApi.Application.Services.Interfaces;
using ChallengeAtmApi.Core.Models;
using ChallengeAtmApi.Infrastructure.Context;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChallengeAtmApi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly PostgresContext _context;
        public TokenService(PostgresContext context)
        {
            _context = context;
        }
        public string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Esta key debería estar en una variable de entorno.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AX,cp+Gf7<])EhIt3?yKA;e]V0[9L30cdGUSjnf,k:tZ0|_M0|%&eKM0L+$wRHD"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "atm-api",
                audience: "atm-api",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<Token?> SaveTokenAsync(string token, Guid authId)
        {
            var t = new Token
            {
                Id = Guid.NewGuid(),
                Token1 = token,
                AuthId = authId
            };
            try
            {
                _context.Tokens.Add(t);
                await _context.SaveChangesAsync();
                return t;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al guardar el token.", ex);
            }
        }
    }
}
