using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VJN.ModelsDTO.UserDTOs;

namespace VJN.Authenticate
{
    public class JwtTokenGenerator
    {
        private readonly JwtSetting _jwtSettings;
        public JwtTokenGenerator(JwtSetting jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateJwtToken(UserDTO ac)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
        new Claim(ClaimTypes.NameIdentifier, ac.UserId.ToString()),
        new Claim(ClaimTypes.Name, ac.UserName),
        new Claim(ClaimTypes.Role, ac.RoleName),
                  // Add custom claims as needed
              }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
