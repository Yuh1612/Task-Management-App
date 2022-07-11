using API.DTOs.Users;

namespace API.DTOs.Projects
{
    public class RemoveMemberResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<UserDTO> Members { get; set; }
    }
}