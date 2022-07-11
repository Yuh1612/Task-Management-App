namespace API.DTOs.Tasks
{
    public class AddAttachmentRequest
    {
        public int taskId { get; set; }
        public IFormFile file { get; set; }
    }
}