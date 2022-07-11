namespace API.DTOs.ListTasks
{
    public class AddListTaskRequest
    {
        public string Name { get; set; }
        public int projectId { get; set; }
    }
}