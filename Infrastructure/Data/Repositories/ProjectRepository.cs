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

        public  async Task<Project?> GetProject(Guid projectId, Guid userId)
        {
            var project = await dbSet.FirstOrDefaultAsync(c => c.Id == projectId);
            if(project != null)
            {
                return project.ProjectMembers.Where(c => c.UserId == userId).Select(c => c.Project).FirstOrDefault();
            }
            return project;
        }
    }
}