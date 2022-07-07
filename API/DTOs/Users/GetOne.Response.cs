using API.DTOs.Projects;

namespace API.DTOs.Users
{
    public class GetOneResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public DateTime? BirthDay { get; set; }

        public List<ProjectDTO> Projects { get; set; }
    }
}