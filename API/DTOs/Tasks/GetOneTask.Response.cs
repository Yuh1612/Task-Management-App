using API.DTOs.Users;

namespace API.DTOs.Tasks
{
    public class GetOneTaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDelete { get; set; }
        public List<TodoDTO> Todos { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
        public List<UserDTO> Members { get; set; }
    }
}