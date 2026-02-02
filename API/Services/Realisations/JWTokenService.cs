using API.Services.Interfaces;
using LogicLayer;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services.Realisations
{
    public class JWTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JWTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Utilisateur user)
        {
            if (user == null)
                throw new Exception("User null");

            var keyString = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyString))
                throw new Exception("Jwt:Key manquant");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login ?? ""),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyString)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(
                    _config.GetValue<int>("Jwt:ExpireMinutes")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
