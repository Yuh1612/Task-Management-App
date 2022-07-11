namespace API.DTOs.Tasks
{
    public class UpdateTaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}