using Domain.Entities.Projects;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        public Task<List<Project>> GetAllByUser(Guid userId);
        public Project? GetProject(Guid projectId, Guid userId);
    }
}