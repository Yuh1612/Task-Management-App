using Domain.Entities.Tasks;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;

namespace Domain.DomainServices
{
    public class TaskManager : ITaskManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Entities.Tasks.Task?> AddTask(Entities.Tasks.Task task)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var currentTask = new Entities.Tasks.Task(task.ListTask, task.Name, task.Description);
                await _unitOfWork.taskRepository.InsertAsync(task);
                await _unitOfWork.CommitTransaction();
                return currentTask;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<bool> AddTodo(Entities.Tasks.Task task, Todo todo)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddTodo(todo.Name, todo.Description, todo.ParentId);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveTodo(Entities.Tasks.Task task, Todo todo)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveTodo(todo);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> DeleteTask(Entities.Tasks.Task task)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.Delete();
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> UpdateTask(Entities.Tasks.Task oldTask, Entities.Tasks.Task newTask)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                oldTask.Update(newTask.Name, newTask.Description);
                _unitOfWork.taskRepository.Update(oldTask);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> AddAttachment(Entities.Tasks.Task task, Attachment attachment)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddAttachment(attachment);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveAttachment(Entities.Tasks.Task task, Attachment attachment)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveAttachment(attachment);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> AddAssignment(Entities.Tasks.Task task, User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddMember(user);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveAssignment(Entities.Tasks.Task task, User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveMember(user);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> AddLabel(Entities.Tasks.Task task, Label label)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddLabel(label);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }

        public async Task<bool> RemoveLabel(Entities.Tasks.Task task, Label label)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveLabel(label);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}