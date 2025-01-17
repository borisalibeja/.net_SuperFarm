using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperFarm.Services
{
    public class JwtTokenService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;

        // Constructor to initialize configuration values
        public JwtTokenService(IConfiguration config)
        {
            _secret = config["Jwt:Secret"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Secret is not configured.");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Issuer is not configured.");
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(config), "Jwt:Audience is not configured.");
        }

        // Method to generate JWT token
        public string GenerateToken(string userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}