using AuthService.Extensions;
using AuthService.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private SecurityKey TokenSecurityKey { get; }
        private int TokenExpiresInSeconds { get; }
        public TokenService(ILogger<TokenService> logger, IConfiguration configuration)
        {
            var tokenSecret = configuration.GetTokenSecret();
            TokenSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            TokenExpiresInSeconds = configuration.GetTokenExpireInSec();
        }

        public string CreateToken(UserModel user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id))
            };

            //claims.Add(new Claim(ClaimTypes.Role, user.Role));

            var signingCredientials = new SigningCredentials(TokenSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(TokenExpiresInSeconds),
                SigningCredentials = signingCredientials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
