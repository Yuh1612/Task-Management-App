using Domain.Entities.Tasks;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TodoRepository : GenericRepository<Todo>, ITodoRepository
    {
        public TodoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Todo>> GetAllByTask(Guid taskId)
        {
            return await dbSet.Where(t => t.Task.Id == taskId && t.ParentId == Guid.Empty).ToListAsync();
        }

        public async Task<List<Todo>> GetAllSubTodosByTodo(Guid parentId)
        {
            return await dbSet.Where(t => t.ParentId == parentId).ToListAsync();
        }
    }
}