using Domain.Entities.Projects;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<List<Project>> GetAllByUser(Guid userId);
    }
}