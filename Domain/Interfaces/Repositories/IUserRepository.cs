using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetOneByUserName(string userName);

        Task<List<User>> GetAllByProject(Guid projectId);

        Task<List<User>> GetAllByTask(Guid taskId);

        Task<bool> IsExistUserName(string userName);
    }
}