using Domain.Entities.ListTasks;
using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ListTaskRepository : GenericRepository<ListTask>, IListTaskRepository
    {
        public ListTaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ListTask>> GetAllByProject(Project project)
        {
            return await dbSet.Where(l => l.Project.Id == project.Id).ToListAsync();
        }

        public async Task<ListTask?> GetOneByTask(int taskId)
        {
            var listTasks = await dbSet.Include(l => l.Tasks).ToListAsync();
            foreach (var listtask in listTasks)
            {
                foreach (var task in listtask.Tasks)
                {
                    if (task.Id == taskId) return listtask;
                }
            }
            return null;
        }
    }
}