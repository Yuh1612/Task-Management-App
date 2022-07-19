using Domain.Entities.Users;

namespace Domain.Interfaces.Authentications
{
    public interface IJwtHandler
    {
        public string GenerateAccessToken(User user);

        public Guid ValidateAccessToken(string accessToken);

        public Guid GetUserId(string accessToken);

        public string GenerateRefreshToken();
    }
}