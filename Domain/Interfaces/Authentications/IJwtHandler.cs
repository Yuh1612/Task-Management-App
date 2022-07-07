using Domain.Entities.Users;

namespace Domain.Interfaces.Authentications
{
    public interface IJwtHandler
    {
        public string GenerateAccessToken(User user);

        public int ValidateAccessToken(string accessToken);

        public string GenerateRefreshToken();
    }
}