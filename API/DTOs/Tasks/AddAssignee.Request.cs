namespace API.DTOs.Tasks
{
    public class AddAssigneeRequest
    {
        public int taskId { get; set; }
        public int userId { get; set; }
    }
}