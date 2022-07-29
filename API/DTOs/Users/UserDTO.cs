using API.DTOs.Projects;

namespace API.DTOs.Users
{
    public record UserDTO(Guid Id = default, string Name = default, string? Email = default);
    public record UserMinDTO(Guid Id = default, string Name = default, string? Email = default, List<ProjectDetailDTO> Projects = default);
    public record UserDetailDTO(Guid Id, string UserName, string Name, string? Email);
    public record CreateUserDTO(string UserName, string Name, string? Email);
    public record UpdateUserDTO(string? Email);
    public record UserAccountDTO(string UserName, string Password);
    public record UserTokenDTO(string AccessToken, string RefreshToken);
}