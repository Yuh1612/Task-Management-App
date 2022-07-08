using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProjectMemberRepository : GenericRepository<ProjectMember>, IProjectMemberRepository
    {
        public ProjectMemberRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsProjectExist(int userId, int projectId)
        {
            var projectMembers = await dbSet.Where(x => x.UserId == userId).ToListAsync();
            return projectMembers.Where(x => x.ProjectId == projectId).Any();
        }

        public async Task<bool> IsProjectExist(int userId, string? projectName)
        {
            if (projectName == null) return false;
            var projectMembers = await dbSet.Include(x => x.Project).Where(x => x.UserId == userId).ToListAsync();
            return projectMembers.Where(x => x.Project.Name == projectName).Any();
        }
    }
}