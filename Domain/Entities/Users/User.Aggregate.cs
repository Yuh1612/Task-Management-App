﻿using Domain.Base;
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
            Password = password;
            Name = name;
            Email = email;
            Age = age;
            BirthDay = birthDay;
            CreateDate = DateTime.UtcNow;
            CreatedById = Id;

            var addEvent = new CreateUserDomainEvent(this);
            AddEvent(addEvent);
        }

        public void Update(int? userId, string? password, string? name, string? email, int? age, DateTime? birthDay)
        {
            Password = password ?? Password;
            Name = name ?? Name;
            Email = email ?? Email;
            Age = age ?? Age;
            BirthDay = birthDay ?? BirthDay;
            UpdateDate = DateTime.UtcNow;
            UpdatedById = userId ?? Id;
        }

        public bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password); ;
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public bool CheckRefreshToken(string refreshToken)
        {
            return RefreshToken.Equals(refreshToken);
        }
    }
}