using API.DTOs.Users;

namespace API.DTOs.Tasks
{
    public class RemoveAssigneeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDTO> Members { get; set; }
    }
}