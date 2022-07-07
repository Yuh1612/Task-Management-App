using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.DomainServices
{
    public interface IProjectDomainService : IDomainService
    {
        public bool IsProjectExist(User user, string name);

        public bool IsMemberExistInProject(User user, Project project);

        public void AddMemberToProject(User user, Project project, bool isCreated = false);
    }
}