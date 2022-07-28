using Domain.Entities.Users;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetOneByUserName(string userName)
        {
            var user = await dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }

        public async Task<List<User>> GetAllByProject(Guid projectId)
        {
            return dbSet.Where(x => x.ProjectMembers.Any(x => x.ProjectId == projectId)).ToList();
        }

        public async Task<List<User>> GetAllByTask(Guid taskId)
        {
            return dbSet.Where(x => x.TaskMembers.Any(x => x.TaskId == taskId)).ToList();
        }

        public Task<bool> IsExistUserName(string userName)
        {
            return dbSet.AnyAsync(x => x.UserName == userName);
        }
    }
}