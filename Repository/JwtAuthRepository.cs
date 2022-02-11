using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Last.Bench.Coder.Beauty.World.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Last.Bench.Coder.Beauty.World.Repository
{
    public class JwtAuthRepository : IJwtAuth
    {
        private readonly string key;

        public JwtAuthRepository(string key)
        {
            this.key = key;
        }
        public string Authentication(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}