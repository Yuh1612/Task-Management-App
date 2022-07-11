namespace API.DTOs.Tasks
{
    public class AddTaskRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int listTaskId { get; set; }
    }
}