using API.DTOs.Tasks;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace API.Services
{
    public class TaskService : BaseService
    {
        private readonly ITaskManager _taskManager;

        public TaskService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper,
            IAuthorizationService authorizationService, ITaskManager taskManager)
            : base(unitOfWork, contextAccessor, mapper, authorizationService)
        {
            _taskManager = taskManager;
        }

        public async Task<TaskDTO> GetOne(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var todos = await _unitOfWork.todoRepository.GetAllByTask(task.Id);
            var members = await _unitOfWork.userRepository.GetAllByTask(task);
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

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, listTask.Project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var task = _mapper.Map<Domain.Entities.Tasks.Task>(request);
            task.ListTask = listTask;
            var created = await _taskManager.AddTask(task);
            if (created == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            return _mapper.Map<TaskDetailDTO>(task);
        }

        public async System.Threading.Tasks.Task UpdateTask(TaskDetailDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _taskManager.UpdateTask(task, _mapper.Map<Domain.Entities.Tasks.Task>(request))) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _taskManager.DeleteTask(task)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task RemoveTodo(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetOneByTodo(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var todo = await _unitOfWork.todoRepository.FindAsync(Id);
            if (todo == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!await _taskManager.RemoveTodo(task, todo)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task AddTodo(CreateTodoDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _taskManager.AddTodo(task, _mapper.Map<Todo>(request))) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task AddAttachment(CreateAttachmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            string? url = await UploadFiles.Upload(request.file);
            if (url == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            var attachment = new Attachment(request.file.FileName, request.file.ContentType, url);
            if (!await _taskManager.AddAttachment(task, attachment)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task RemoveAttachment(Guid Id)
        {
            var task = await _unitOfWork.taskRepository.GetOneByAttachment(Id);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var attachment = await _unitOfWork.attachmentRepository.FindAsync(Id);
            if (attachment == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!await _taskManager.RemoveAttachment(task, attachment)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task AddMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (task.HasMember(user)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _taskManager.AddAssignment(task, user)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task RemoveMember(AssigmentDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var user = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!task.HasMember(user)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _taskManager.RemoveAssignment(task, user)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task AddLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _taskManager.AddLabel(task, label)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async System.Threading.Tasks.Task RemoveLabel(TaskLabelDTO request)
        {
            var task = await _unitOfWork.taskRepository.FindAsync(request.taskId);
            if (task == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, task, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var label = await _unitOfWork.labelRepository.FindAsync(request.labelId);
            if (label == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!task.HasLabel(label)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _taskManager.RemoveLabel(task, label)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}