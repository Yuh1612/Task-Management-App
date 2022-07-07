using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Users
{
    public class GetOneRequest
    {
        [Required]
        public int Id { get; set; }
    }
}