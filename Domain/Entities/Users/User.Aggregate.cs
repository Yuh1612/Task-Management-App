using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Users.Events;
using MediatR;

namespace Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(string userName, string password, string name, string? email, int? age, DateTime? birthDay)
        {
            UserName = userName;
            Password = BCrypt.Net.BCrypt.HashPassword(password);
            Name = name;
            Email = email;
            Age = age;
            BirthDay = birthDay;
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;

            var addEvent = new CreateUserDomainEvent(this);
            AddEvent(addEvent);
        }

        public void Update(string? password, string? name, string? email, int? age, DateTime? birthDay)
        {
            Password = password == null ? Password : BCrypt.Net.BCrypt.HashPassword(password);
            Name = name ?? Name;
            Email = email ?? Email;
            Age = age ?? Age;
            BirthDay = birthDay ?? BirthDay;
            UpdateDate = DateTime.UtcNow;
        }

        public bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiredDay = DateTime.UtcNow.AddDays(2);
            UpdateDate = DateTime.UtcNow;
        }

        public bool CheckRefreshToken(string refreshToken)
        {
            return RefreshToken.Equals(refreshToken);
        }

        public bool CheckRefreshTokenExpired()
        {
            return RefreshTokenExpiredDay <= DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDelete = true;
            UpdateDate = DateTime.UtcNow;
        }

        public bool IsMember(Project project)
        {
            return ProjectMembers.Where(m => m.ProjectId == project.Id).Any();
        }
    }
}