using API.DTOs.Projects;

namespace API.DTOs.Users
{
    public record UserDTO(Guid Id = default, string Name = default, string? Email = default, List<ProjectDetailDTO> Projects = default);
    public record UserMinDTO(Guid Id, string Name, string? Email);
    public record UserDetailDTO(Guid Id, string UserName, string Name, string? Email, int? Age, DateTime? BirthDay);
    public record CreateUserDTO(string UserName, string Password, string Name, string? Email, int? Age, DateTime? BirthDay);
    public record UpdateUserDTO(string? Password, string? Name, string? Email, int? Age, DateTime? BirthDay);
    public record UserAccountDTO(string UserName, string Password);
    public record UserTokenDTO(string AccessToken, string RefreshToken);
}