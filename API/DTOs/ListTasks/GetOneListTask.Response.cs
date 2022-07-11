using API.DTOs.Tasks;

namespace API.DTOs.ListTasks
{
    public class GetOneListTaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }
        public List<TaskDTO> Tasks { get; set; }
    }
}