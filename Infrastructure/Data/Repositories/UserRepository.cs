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

        public async Task<User?> FindOneByUserName(string userName)
        {
            var user = await dbSet.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetAllByProject(Project project)
        {
            var users = await dbSet.Include(x => x.ProjectMembers).ToListAsync();

            return users.Where(x => x.IsMember(project) == true).ToList();
        }
    }
}