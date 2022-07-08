namespace API.DTOs.Projects
{
    public class UpdateProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}