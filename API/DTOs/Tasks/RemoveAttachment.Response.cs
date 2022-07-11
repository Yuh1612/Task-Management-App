namespace API.DTOs.Tasks
{
    public class RemoveAttachmentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
    }
}