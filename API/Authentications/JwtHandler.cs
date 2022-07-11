using Domain.Entities.Users;
using Domain.Interfaces.Authentications;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Authentications
{
    public class JwtHandler : IJwtHandler
    {
        public string GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(AppSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim(ClaimTypes.Name, user.Name),
                }),

                Expires = DateTime.UtcNow.AddMinutes(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];

            using (var rd = RandomNumberGenerator.Create())
            {
                rd.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public int ValidateAccessToken(string accessToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(AppSettings.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecretKey)),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false,
            };
            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(
                    accessToken,
                    tokenValidationParameters,
                    out var validatedToken
                );

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase
                    );
                    if (!result) throw new Exception("Invalid Token");
                }

                var userId = tokenInVerification.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
                if (userId == null) throw new Exception("Invalid Token");

                var utcExpireDate = long.Parse(tokenInVerification.Claims.
                    FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow) throw new Exception("AccessToken still available");

                return Int32.Parse(userId);
            }
            catch
            {
                throw;
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        }
    }
}