using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        public Task<List<Project>> GetAllByUser(User user);

        public Task<Project?> GetOneByListTask(int listTaskId);
    }
}