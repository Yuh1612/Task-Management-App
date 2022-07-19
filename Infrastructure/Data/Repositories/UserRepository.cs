using Domain.Entities.Projects;
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

        public async Task<List<User>> GetAllByProject(Project project)
        {
            var users = await dbSet.ToListAsync();
            return users.Where(x => x.HasProject(project) == true).ToList();
        }

        public async Task<List<User>> GetAllByTask(Domain.Entities.Tasks.Task task)
        {
            var users = await dbSet.ToListAsync();
            return users.Where(x => x.HasTask(task) == true).ToList();
        }

        public Task<bool> IsExistUserName(string userName)
        {
            return dbSet.AnyAsync(x => x.UserName == userName);
        }
    }
}