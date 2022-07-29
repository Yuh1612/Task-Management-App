using Domain.Entities.Projects;

namespace Domain.Interfaces.Repositories
{
    public interface IListTaskRepository : IGenericRepository<ListTask>
    {
        Task<ListTask?> GetListTask(Guid listTaskId, Guid userId);
    }
}