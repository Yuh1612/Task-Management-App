namespace API.DTOs.Projects
{
    public class RemoveMemberRequest
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}