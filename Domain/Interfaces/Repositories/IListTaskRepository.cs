using Domain.Entities.Projects;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository : IGenericRepository<ListTask>
    {
        public Task<List<ListTask>> GetAllByProject(Project project);

        public Task<ListTask> GetOneByTask(Guid taskId);
    }
}