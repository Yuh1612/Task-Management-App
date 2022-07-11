using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetOneByUserName(string userName);

        public Task<List<User>> GetAllByProject(Project project);

        public Task<List<User>> GetAllByTask(Entities.Tasks.Task task);
    }
}