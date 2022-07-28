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

        public async Task<TaskDTO> GetOne(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetTask(Id, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound, task);

            var todos = await _unitOfWork.todoRepository.GetAllByTask(task.Id);
            var members = await _unitOfWork.userRepository.GetAllByTask(task.Id);
            var response = _mapper.Map<TaskDTO>(task);
            _mapper.Map(todos, response.Todos);
            foreach (var todo in response.Todos)
            {
                var subtodo = await _unitOfWork.todoRepository.GetAllSubTodosByTodo(todo.Id);
                _mapper.Map(subtodo, todo.SubTodos);
            }
            _mapper.Map(members, response.Members);
            return response;
        }

        public async Task<TaskDetailDTO> AddTask(CreateTaskDTO request)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(request.listTaskId);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (await _unitOfWork.projectRepository.GetProject(listTask.Project.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden);


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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task UpdateTask(TaskDetailDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.Id, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetTask(Id, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task RemoveTodo(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetOneByTodo(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (await _unitOfWork.taskRepository.GetTask(task.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var todo = await _unitOfWork.todoRepository.FindAsync(Id);
            if (todo == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task AddTodo(CreateTodoDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task AddAttachment(CreateAttachmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            string? url = await UploadFiles.Upload(request.file);
            if (url == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task RemoveAttachment(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetOneByAttachment(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (await _unitOfWork.taskRepository.GetTask(task.Id, GetCurrentUserId()) == null) throw new HttpResponseException(HttpStatusCode.Forbidden);


            var attachment = await _unitOfWork.attachmentRepository.FindAsync(Id);
            if (attachment == null) throw new HttpResponseException(HttpStatusCode.NotFound);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task AddMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (task.HasMember(user.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                await _unitOfWork.BeginTransaction();
                task.AddMember(user);
                _unitOfWork.taskRepository.Update(task);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task RemoveMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!task.HasMember(user.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task AddLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async System.Threading.Tasks.Task RemoveLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.GetTask(request.taskId, GetCurrentUserId());
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest);

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
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}