﻿using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Users.Events;

namespace Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(string userName, string password, string name, string? email, int? age, DateTime? birthDay)
        {
            if (Id == Guid.Empty) Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
            Name = name;
            Email = email;
            Age = age;
            BirthDay = birthDay;
            ProjectMembers = new HashSet<ProjectMember>();
        }

        public void AddCreateUserDomainEvent()
        {
            var newEvent = new CreateUserDomainEvent(this);
            AddEvent(newEvent);
        }

        public void Update(string? password, string? name, string? email, int? age, DateTime? birthDay)
        {
            Password = password == null ? Password : BCrypt.Net.BCrypt.HashPassword(password);
            Name = name ?? Name;
            Email = email ?? Email;
            Age = age ?? Age;
            BirthDay = birthDay ?? BirthDay;
        }

        public bool HasPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public void HashPassWord()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
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