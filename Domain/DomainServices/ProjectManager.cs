using Domain.Entities.Projects;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;

namespace Domain.DomainServices
{
    public class ProjectManager : IProjectManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project?> CreateProject(User user, Project project)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var currentProject = new Project(project.Name, project.Description);
                currentProject.AddMember(user, true);
                await _unitOfWork.projectRepository.InsertAsync(currentProject);
                currentProject.AddListTask(new ListTask("Planning"));
                currentProject.AddListTask(new ListTask("To-do"));
                currentProject.AddListTask(new ListTask("Doing"));
                await _unitOfWork.CommitTransaction();
                return currentProject;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<bool> DeleteProject(Project project)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                project.Delete();
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> AddMember(Project project, User member)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                project.AddMember(member);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveMember(Project project, User member)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                project.RemoveMember(member);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> UpdateProject(Project oldProject, Project newProject)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                oldProject.Update(newProject.Name, newProject.Description);
                _unitOfWork.projectRepository.Update(oldProject);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> CreateListTask(Project project, ListTask listTask)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                project.AddListTask(listTask);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveListTask(Project project, ListTask listTask)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                project.RemoveListTask(listTask);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> UpdateListTask(ListTask oldListTask, ListTask newListTask)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                oldListTask.Update(newListTask.Name, newListTask.Color);
                _unitOfWork.listTaskRepository.Update(oldListTask);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}