namespace API.DTOs.Tasks
{
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string StorageUrl { get; set; }
    }
}