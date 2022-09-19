using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.AuthorizationAndAuthentication
{
    public class GenerateToken
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public GenerateToken(TokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        public string GenerateJwt(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenConfiguration.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = new SecurityTokenDescriptor
            { 
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                Expires = DateTime.UtcNow.AddHours(_tokenConfiguration.ExpirationHours),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,  _tokenConfiguration.Sub),
                    new Claim("Module", _tokenConfiguration.Module)
                })
            };

            var token = tokenHandler.CreateToken(jwtToken);
            return tokenHandler.WriteToken(token);
        }
    }
}
