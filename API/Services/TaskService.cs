using API.DTOs.Tasks;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Tasks;
using Domain.Interfaces;
using System.Net;

namespace API.Services
{
    public class TaskService : BaseService
    {
        public TaskService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
            : base(unitOfWork, contextAccessor, mapper)
        {
        }

        public async Task<TaskDTO> GetOne(Guid taskId)
        {
            var task = await _unitOfWork.taskRepository.GetTask(taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            var response = _mapper.Map<TaskDTO>(task);
            _mapper.Map(task.Todos.Where(s => s.ParentId == Guid.Empty).Select(s => s), response.Todos);
            _mapper.Map(task.TaskMembers.Select(s => s.User), response.Members);
            foreach (var todo in response.Todos)
            {
                _mapper.Map(task.Todos.Where(s => s.ParentId != Guid.Empty), todo.SubTodos);
            }
            return response;
        }

        public async Task<TaskDetailDTO> AddTask(CreateTaskDTO request)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(request.listTaskId);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Listtask is not found!");

            if (await _unitOfWork.projectRepository.GetProject(listTask.Project.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden, "User is not a member in this project!");

            try
            {
                await _unitOfWork.BeginTransaction();
                var task = new Domain.Entities.Tasks.Task(listTask, request.Name, request.Description);
                await _unitOfWork.taskRepository.InsertAsync(task);
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<TaskDetailDTO>(task);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task UpdateTask(TaskDetailDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.Id, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.Update(request.Name, request.Description);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid taskId)
        {
            var task = await _unitOfWork.taskRepository.GetTask(taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.Delete();
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task RemoveTodo(Guid todoId)
        {
            var task = await _unitOfWork.taskRepository.GetOneByTodo(todoId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            if (await _unitOfWork.taskRepository.GetTask(task.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden, "User is not a member in this project!");

            var todo = await _unitOfWork.todoRepository.FindAsync(todoId);
            if (todo == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Todo is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveTodo(todo);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task AddTodo(CreateTodoDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddTodo(request.Name, request.Description, request.ParentId);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task AddAttachment(CreateAttachmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found");

            string? url = await UploadFiles.Upload(request.file);
            if (url == null) throw new HttpResponseException(HttpStatusCode.BadRequest, "Url is invalid!");

            try
            {
                var attachment = new Attachment(request.file.FileName, request.file.ContentType, url);
                await _unitOfWork.BeginTransaction();
                task.AddAttachment(attachment);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task RemoveAttachment(Guid attachmentId)
        {
            var task = await _unitOfWork.taskRepository.GetOneByAttachment(attachmentId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            if (await _unitOfWork.taskRepository.GetTask(task.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden, "User is not a member in this project!");

            var attachment = await _unitOfWork.attachmentRepository.FindAsync(attachmentId);
            if (attachment == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Attachment is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveAttachment(attachment);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task AddMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");

            if (task.HasMember(user.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest, "User is already existed in this task!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddMember(user.Id);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task RemoveMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");

            if (!task.HasMember(user.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest, "User is not a member in this task!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveMember(user);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task AddLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Label is not found");

            if (task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest, "Label is already existed in this task!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddLabel(label);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async System.Threading.Tasks.Task RemoveLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Task is not found!");

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Label is not found!");

            if (!task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest, "Label is not existed!");

            try
            {
                await _unitOfWork.BeginTransaction();
                task.RemoveLabel(label);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }
    }
}