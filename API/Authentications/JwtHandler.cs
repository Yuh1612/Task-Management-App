using API.Extensions;
using Domain.Entities.Users;
using Domain.Interfaces.Authentications;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
                    new Claim("username", user.UserName),
                    new Claim("id", user.Id.ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature
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

        public Guid GetUserId(string accessToken)
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
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase
                    );
                    if (!result) throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var userId = tokenInVerification.Claims.First(x => x.Type == "UserId").Value;
                if (userId == null) throw new HttpResponseException(HttpStatusCode.Unauthorized);

                return Guid.Parse(userId);
            }
            catch
            {
                throw;
            }
        }

        public Guid ValidateAccessToken(string accessToken)
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
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase
                    );
                    if (!result) throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                var userId = tokenInVerification.Claims.First(x => x.Type == "UserId").Value;
                if (userId == null) throw new HttpResponseException(HttpStatusCode.Unauthorized);

                var utcExpireDate = long.Parse(tokenInVerification.Claims.
                    First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow) throw new HttpResponseException(HttpStatusCode.Unauthorized);

                return Guid.Parse(userId);
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