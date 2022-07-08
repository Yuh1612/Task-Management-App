using Domain.Entities.Projects;
using Domain.Entities.Users;
using Domain.Interfaces.DomainServices;

namespace Domain.DomainServices
{
    public class ProjectDomainService : IProjectDomainService
    {
        public void AddMemberToProject(User user, Project project, bool isCreated = false)
        {
            if (IsMemberExistInProject(user, project))
            {
                throw new Exception("This member already exist.");
            }
            project.ProjectMembers.Add(new ProjectMember
            {
                User = user,
                Project = project,
                IsCreated = isCreated,
                CreateDate = DateTime.Now,
                CreatedById = user.Id,
                UpdateDate = DateTime.Now,
                UpdatedById = user.Id,
            });
        }

        public bool IsMemberExistInProject(User user, Project project)
        {
            return project.ProjectMembers.Any(x => x.UserId == user.Id);
        }
    }
}