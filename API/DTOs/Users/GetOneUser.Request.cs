using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Users
{
    public class GetOneUserRequest
    {
        [Required]
        public int Id { get; set; }
    }
}