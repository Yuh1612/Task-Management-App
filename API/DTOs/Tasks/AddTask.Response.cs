namespace API.DTOs.Tasks
{
    public class AddTaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}