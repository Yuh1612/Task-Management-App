namespace API.DTOs.Projects
{
    public class AddMemberRequest
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}