namespace API.DTOs.Tasks
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}