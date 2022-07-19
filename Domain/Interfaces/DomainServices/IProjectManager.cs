using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.DomainServices
{
    public interface IProjectManager
    {
        public Task<Project?> CreateProject(User user, Project project);

        public Task<bool> UpdateProject(Project oldProject, Project newProject);

        public Task<bool> DeleteProject(Project project);

        public Task<bool> AddMember(Project project, User member);

        public Task<bool> RemoveMember(Project project, User member);

        public Task<bool> CreateListTask(Project project, ListTask listTask);

        public Task<bool> RemoveListTask(Project project, ListTask listTask);

        public Task<bool> UpdateListTask(ListTask oldListTask, ListTask newListTask);
    }
}