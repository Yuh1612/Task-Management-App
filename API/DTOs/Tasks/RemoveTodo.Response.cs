namespace API.DTOs.Tasks
{
    public class RemoveTodoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<TodoDTO> Todos { get; set; }
    }
}