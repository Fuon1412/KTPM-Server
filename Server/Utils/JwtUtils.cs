using Microsoft.IdentityModel.Tokens;
using Server.Interfaces.IUltilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtUtils(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            _key = jwtSettings.GetValue<string>("Key") ?? throw new ArgumentNullException(nameof(_key), "JWT key is missing.");
            _issuer = jwtSettings.GetValue<string>("Issuer") ?? throw new ArgumentNullException(nameof(_issuer), "JWT issuer is missing.");
            _audience = jwtSettings.GetValue<string>("Audience") ?? throw new ArgumentNullException(nameof(_audience), "JWT audience is missing.");
            _expirationMinutes = jwtSettings.GetValue<int>("ExpirationMinutes");
        }

        public string GenerateToken(Guid accountId, string role, Guid userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, accountId.ToString()),
                new(ClaimTypes.Role, role),
                new(ClaimTypes.Name, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
