using Domain.Entities.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITodoRepository : IGenericRepository<Todo>
    {
        Task<List<Todo>> GetAllByTask(Guid taskId);

        Task<List<Todo>> GetAllSubTodosByTodo(Guid parentId);
    }
}