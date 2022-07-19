using Domain.Entities.Projects;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Entities.Tasks.Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Domain.Entities.Tasks.Task>> GetAllByListTask(ListTask listTask)
        {
            var tasks = dbSet.Where(t => t.ListTask.Id == listTask.Id);
            return await tasks.ToListAsync();
        }

        public async Task<Domain.Entities.Tasks.Task> GetOneByAttachment(Guid attachmentId)
        {
            var task = await dbSet.FirstOrDefaultAsync(t => t.Attachments.Any(t => t.Id == attachmentId) == true);
            if (task == null) throw new KeyNotFoundException(nameof(task));
            return task;
        }

        public async Task<Domain.Entities.Tasks.Task> GetOneByTodo(Guid todoId)
        {
            var task = await dbSet.FirstOrDefaultAsync(t => t.Todos.Any(t => t.Id == todoId) == true);
            if (task == null) throw new KeyNotFoundException(nameof(task));
            return task;
        }
    }
}