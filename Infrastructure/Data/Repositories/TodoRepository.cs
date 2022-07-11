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

        public async Task<List<Todo>> GetAllByTask(int taskId)
        {
            return await dbSet.Where(t => t.Task.Id == taskId && t.ParentId == null).ToListAsync();
        }

        public async Task<List<Todo>> GetAllSubTodosByTodo(int parentId)
        {
            return await dbSet.Where(t => t.ParentId == parentId).ToListAsync();
        }
    }
}