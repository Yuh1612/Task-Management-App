using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> FindOneByUserName(string userName);

        public Task<List<User>> GetAllByProject(Project project);
    }
}