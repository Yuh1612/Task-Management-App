using Domain.Entities.ListTasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<Entities.Tasks.Task>
    {
        public Task<List<Entities.Tasks.Task>> GetAllByListTask(ListTask listTask);

        public Task<Entities.Tasks.Task> GetOneByTodo(int todoId);

        public Task<Entities.Tasks.Task> GetOneByAttachment(int attachmentId);
    }
}