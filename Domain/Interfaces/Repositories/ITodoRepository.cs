using Domain.Entities.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITodoRepository : IGenericRepository<Todo>
    {
        public Task<List<Todo>> GetAllByTask(Guid taskId);

        public Task<List<Todo>> GetAllSubTodosByTodo(Guid parentId);
    }
}