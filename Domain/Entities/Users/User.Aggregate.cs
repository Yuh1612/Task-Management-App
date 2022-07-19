using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using Domain.Entities.Users.Events;

namespace Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(string userName, string password, string name, string? email, int? age, DateTime? birthDay)
        {
            if (Id == Guid.Empty) Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
            Name = name;
            Email = email;
            Age = age;
            BirthDay = birthDay;
            ProjectMembers = new HashSet<ProjectMember>();

            var newEvent = new CreateUserDomainEvent(this);
            AddEvent(newEvent);
        }

        public void Update(string? password, string? name, string? email, int? age, DateTime? birthDay)
        {
            Password = password == null ? Password : BCrypt.Net.BCrypt.HashPassword(password);
            Name = name ?? Name;
            Email = email ?? Email;
            Age = age ?? Age;
            BirthDay = birthDay ?? BirthDay;
        }

        public bool HasUserName(string username)
        {
            return UserName.Equals(username);
        }

        public bool HasPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public void HashPassWord()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public void UpdateRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiredDay = DateTime.UtcNow.AddDays(2);
        }

        public bool HasRefreshToken(string refreshToken)
        {
            return RefreshToken.Equals(refreshToken);
        }

        public bool IsRefreshTokenExpired()
        {
            return RefreshTokenExpiredDay <= DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDelete = true;
        }

        public bool HasProject(Project project)
        {
            return ProjectMembers.Any(m => m.ProjectId == project.Id);
        }

        public bool HasTask(Tasks.Task task)
        {
            return TaskMembers.Any(m => m.TaskId == task.Id);
        }

        public bool HasProject(string? name)
        {
            return ProjectMembers.Any(m => m.Project.HasName(name));
        }

        public bool HasProject(Guid id)
        {
            return ProjectMembers.Any(m => m.Project.Id == id);
        }

        internal void AddProject(ProjectMember projectMember)
        {
            ProjectMembers.Add(projectMember);
        }

        internal void AddTask(TaskMember taskMember)
        {
            TaskMembers.Add(taskMember);
        }
    }
}