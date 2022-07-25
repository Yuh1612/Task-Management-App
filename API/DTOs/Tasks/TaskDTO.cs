using API.DTOs.Users;

namespace API.DTOs.Tasks
{
    public record AttachmentDTO(Guid Id = default, string FileName = default, string Type = default, string StorageUrl = default);
    public record CreateAttachmentDTO(Guid taskId, IFormFile file);
    public record CreateTodoDTO(Guid taskId, string Name, Guid ParentId, string? Description);
    public record TodoDTO(Guid Id, string Name, string? Description, bool IsDone, List<SubTodoDTO> SubTodos);
    public record SubTodoDTO(Guid Id, string Name, string? Description, bool IsDone);
    public record LabelDTO(Guid Id, string Title, string Color);
    public record AssigmentDTO(Guid taskId, Guid userId);
    public record TaskLabelDTO(Guid taskId, Guid labelId);

    public record TaskDTO(Guid Id = default, string Name = default, string? Description = default, List<TodoDTO> Todos = default, List<UserMinDTO> Members = default, List<AttachmentDTO> Attachments = default, List<LabelDTO> Labels = default);
    public record TaskDetailDTO(Guid Id, string? Name, string? Description);
    public record CreateTaskDTO(Guid listTaskId, string Name, string? Description);
}