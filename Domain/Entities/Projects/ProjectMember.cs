using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Projects
{
    public class ProjectMember : Entity
    {
        public ProjectMember()
        {
        }

        public ProjectMember(Project project, User user, bool isCreated)
        {
            Project = project;
            User = user;
            IsCreated = isCreated;
        }

        public Guid? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsCreated { get; set; }
    }
}