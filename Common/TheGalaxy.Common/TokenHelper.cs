using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TheGalaxy.Common
{
    public static class TokenHelper
    {
        public static string CreateUserAccessToken(IConfiguration configuration, Guid userId, string roleName)
        {
            var result = string.Empty;
            var lifeSpan = configuration["USER_ACCESS_LIFE_TIME"];
            if (!string.IsNullOrWhiteSpace(lifeSpan))
            {
                int.TryParse(lifeSpan, out var lifeTimeInt);
                var lifeTimeSpan = TimeSpan.FromMinutes(lifeTimeInt);

                var expiresIn = DateTime.UtcNow.Add(lifeTimeSpan);
                var claims = GetUserClaims(userId, expiresIn, roleName);
                return GetToken(configuration, claims, expiresIn);
            }

            return result;
        }

        private static string GetToken(IConfiguration configuration, ClaimsIdentity claims, DateTime expiresIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = configuration["ACCESS_TOKEN_ISSUER"],
                Audience = configuration["ACCESS_TOKEN_AUDIENCE"],
                NotBefore = DateTime.UtcNow,
                Expires = expiresIn,
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(configuration), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["ACCESS_TOKEN_SECRET_KEY"]));
        }

        private static ClaimsIdentity GetUserClaims(Guid userId, DateTime expiresIn, string role)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("expiresIn", StringHelper.DateInJSFormat(expiresIn))
            });
        }

    }
}
