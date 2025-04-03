using ChallengeAtmApi.Context;
using ChallengeAtmApi.Models;
using ChallengeAtmApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChallengeAtmApi.Services
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("keyultrasecreta"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
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
