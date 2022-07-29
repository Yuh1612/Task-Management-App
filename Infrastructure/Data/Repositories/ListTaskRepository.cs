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

        public async Task<ListTask?> GetListTask(Guid listTaskId, Guid userId)
        {
            var listTask = await dbSet.FirstOrDefaultAsync(c => c.Id == listTaskId);
            if (listTask != null)
            {
                return listTask.Project.ProjectMembers.Any(c => c.UserId == userId) ? listTask : null;
            }
            return listTask;
        }
    }
}