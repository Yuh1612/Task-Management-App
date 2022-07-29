using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using Domain.Entities.Users.Events;

namespace Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(string userName, string? email)
        {
            if (Id == Guid.Empty) Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            ProjectMembers = new HashSet<ProjectMember>();
        }

        public void AddCreateUserDomainEvent()
        {
            var newEvent = new CreateUserDomainEvent(this);
            AddEvent(newEvent);
        }

        public void Update(string? email)
        {
            Email = email ?? Email;
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiredDay = DateTime.UtcNow.AddDays(2);
        }

        public bool HasRefreshToken(string refreshToken)
        {
            return RefreshToken.Equals(refreshToken);
        }

        public bool IsRefreshTokenExpired()
        {
            return RefreshTokenExpiredDay <= DateTime.UtcNow;
        }
    }
}