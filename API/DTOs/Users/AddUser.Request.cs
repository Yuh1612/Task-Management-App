using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Users
{
    public class AddUserRequest
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        public int? Age { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}