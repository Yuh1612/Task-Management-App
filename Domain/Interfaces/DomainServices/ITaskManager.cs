using Domain.Entities.Tasks;
using Domain.Entities.Users;

namespace Domain.Interfaces.DomainServices
{
    public interface ITaskManager
    {
        public Task<Entities.Tasks.Task?> AddTask(Entities.Tasks.Task task);

        public Task<bool> UpdateTask(Entities.Tasks.Task oldTask, Entities.Tasks.Task newTask);

        public Task<bool> DeleteTask(Entities.Tasks.Task task);

        public Task<bool> AddTodo(Entities.Tasks.Task task, Todo todo);

        public Task<bool> RemoveTodo(Entities.Tasks.Task task, Todo todo);

        public Task<bool> AddAttachment(Entities.Tasks.Task task, Attachment attachment);

        public Task<bool> RemoveAttachment(Entities.Tasks.Task task, Attachment attachment);

        public Task<bool> AddAssignment(Entities.Tasks.Task task, User user);

        public Task<bool> RemoveAssignment(Entities.Tasks.Task task, User user);

        public Task<bool> AddLabel(Entities.Tasks.Task task, Label label);

        public Task<bool> RemoveLabel(Entities.Tasks.Task task, Label label);
    }
}