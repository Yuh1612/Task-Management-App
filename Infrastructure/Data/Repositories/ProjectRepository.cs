using Domain.Entities.Projects;
using Domain.Entities.Users;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Project>> GetAllByUser(Guid userId)
        {
            return await dbSet.Where(x => x.ProjectMembers.Any(x => x.UserId == userId)).ToListAsync();
        }
    }
}