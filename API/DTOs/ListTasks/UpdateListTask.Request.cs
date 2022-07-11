namespace API.DTOs.ListTasks
{
    public class UpdateListTaskRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }
}