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

        public async Task<List<Project>> GetAllByUser(User user)
        {
            var projects = await dbSet.Include(x => x.ProjectMembers).ToListAsync();

            return projects.Where(x => x.HasOwner(user) == true).ToList();
        }

        public async Task<Project> GetOneByListTask(Guid listTaskId)
        {
            var projects = await dbSet.Include(x => x.ListTasks).ToListAsync();
            foreach (var project in projects)
            {
                foreach (var listtask in project.ListTasks)
                {
                    if (listtask.Id == listTaskId) return project;
                }
            }
            throw new KeyNotFoundException(nameof(listTaskId));
        }
    }
}