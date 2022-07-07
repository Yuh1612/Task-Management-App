﻿namespace API.DTOs.Users
{
    public class AddUserResponse
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string? Email { get; set; }

        public int? Age { get; set; }

        public DateTime? BirthDay { get; set; }
    }
}