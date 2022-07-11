namespace API.DTOs.Tasks
{
    public class AddTodoRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public int TaskId { get; set; }
    }
}