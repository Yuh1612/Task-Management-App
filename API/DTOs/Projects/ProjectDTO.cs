using API.DTOs.Tasks;
using API.DTOs.Users;

namespace API.DTOs.Projects
{
    public record ProjectDTO(Guid Id = default, string Name = default, string? Description = default, List<UserMinDTO> Members = default, List<ListTaskDetailDTO> ListTasks = default);
    public record ProjectDetailDTO(Guid Id, string Name, string? Description);
    public record CreateProjectDTO(string Name, string? Description);
    public record ProjectMemberDTO(Guid projectId, Guid userId);
    public record ListTaskDTO(Guid Id = default, string Name = default, string? Color = default, List<TaskDetailDTO> Tasks = default);
    public record ListTaskDetailDTO(Guid Id = default, string? Name = default, string? Color = default);
    public record CreateListTaskDTO(Guid projectId, string Name, string? Color);
}