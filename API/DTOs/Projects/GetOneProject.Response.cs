using API.DTOs.ListTasks;
using API.DTOs.Users;

namespace API.DTOs.Projects
{
    public class GetOneProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<UserDTO> Members { get; set; }
        public List<ListTaskDTO> ListTasks { get; set; }
    }
}