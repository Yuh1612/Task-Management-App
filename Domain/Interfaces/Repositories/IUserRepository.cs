using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetOneByUserName(string userName);

        public Task<List<User>> GetAllByProject(Guid projectId);

        public Task<List<User>> GetAllByTask(Guid taskId);

        public Task<bool> IsExistUserName(string userName);
    }
}