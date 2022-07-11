namespace API.DTOs.Tasks
{
    public class TodoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public List<SubTodoDTO> SubTodos { get; set; }
    }
}