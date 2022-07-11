using API.DTOs.Tasks;
using API.DTOs.Users;
using AutoMapper;
using Domain.Interfaces;

namespace API.Services
{
    public class TaskService : BaseService
    {
        private readonly IMapper _mapper;
        private readonly ListTaskService _listTaskService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, ListTaskService listTaskService) : base(unitOfWork)
        {
            _mapper = mapper;
            _listTaskService = listTaskService;
        }

        public async Task<AddTaskResponse> AddTask(int? userId, AddTaskRequest request)
        {
            var user = await CheckUser(userId);

            var listTask = await UnitOfWork.listTaskRepository.FindAsync(request.listTaskId);

            if (listTask.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(listTask));

            var task = new Domain.Entities.Tasks.Task(user, listTask, request.Name, request.Description);

            try
            {
                await UnitOfWork.BeginTransaction();
                await UnitOfWork.taskRepository.InsertAsync(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var response = _mapper.Map<AddTaskResponse>(task);
            return response;
        }

        public async Task<UpdateTaskResponse> UpdateTask(int? userId, UpdateTaskRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.Id);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.Update(user, request.Name, request.Description);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var response = _mapper.Map<UpdateTaskResponse>(task);
            return response;
        }

        public async Task<GetOneTaskResponse> GetOne(int? userId, GetOneTaskRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.Id);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            var todos = await UnitOfWork.todoRepository.GetAllByTask(task.Id);
            var members = await UnitOfWork.userRepository.GetAllByTask(task);
            var response = _mapper.Map<GetOneTaskResponse>(task);
            response.Todos = _mapper.Map<List<TodoDTO>>(todos);
            foreach (TodoDTO item in response.Todos)
            {
                item.SubTodos = _mapper.Map<List<SubTodoDTO>>(await UnitOfWork.todoRepository.GetAllSubTodosByTodo(item.Id));
            }
            response.Members = _mapper.Map<List<UserDTO>>(members);
            return response;
        }

        public async Task<RemoveTodoResponse> RemoveTodo(int? userId, RemoveTodoRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.GetOneByTodo(request.Id);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.RemoveTodo(user, request.Id);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var todos = await UnitOfWork.todoRepository.GetAllByTask(task.Id);
            var response = _mapper.Map<RemoveTodoResponse>(task);
            response.Todos = _mapper.Map<List<TodoDTO>>(todos);
            foreach (TodoDTO item in response.Todos)
            {
                item.SubTodos = _mapper.Map<List<SubTodoDTO>>(await UnitOfWork.todoRepository.GetAllSubTodosByTodo(item.Id));
            }
            return response;
        }

        public async Task<AddTodoResponse> AddTodo(int? userId, AddTodoRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.TaskId);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.AddTodo(user, request.Name, request.Description, request.ParentId);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var todos = await UnitOfWork.todoRepository.GetAllByTask(task.Id);
            var response = _mapper.Map<AddTodoResponse>(task);
            response.Todos = _mapper.Map<List<TodoDTO>>(todos);
            foreach (TodoDTO item in response.Todos)
            {
                item.SubTodos = _mapper.Map<List<SubTodoDTO>>(await UnitOfWork.todoRepository.GetAllSubTodosByTodo(item.Id));
            }
            return response;
        }

        public async Task<AddAttachmentResponse> AddAttachment(int? userId, AddAttachmentRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.taskId);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            try
            {
                string? url = await UploadFiles.Upload(request.file);
                if (url == null) throw new FileNotFoundException();

                await UnitOfWork.BeginTransaction();
                task.AddAttachment(user, request.file.FileName, request.file.ContentType, url);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var response = _mapper.Map<AddAttachmentResponse>(task);

            return response;
        }

        public async Task<RemoveAttachmentResponse> RemoveAttachment(int? userId, RemoveAttachmentRequest request)
        {
            var user = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.GetOneByAttachment(request.Id);

            if (task.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.RemoveAttachment(user, request.Id);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }

            var response = _mapper.Map<RemoveAttachmentResponse>(task);

            return response;
        }

        public async Task<AddAssgineeResponse> AddMember(int? userId, AddAssigneeRequest request)
        {
            var currentUser = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.taskId);

            if (task.HasOwner(currentUser) == false) throw new DirectoryNotFoundException(nameof(task));

            var user = await UnitOfWork.userRepository.FindAsync(request.userId);

            if (task.HasMember(user) == true) throw new ArgumentException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.AddMember(user);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
            var members = await UnitOfWork.userRepository.GetAllByTask(task);
            var response = _mapper.Map<AddAssgineeResponse>(task);
            response.Members = _mapper.Map<List<UserDTO>>(members);
            return response;
        }

        public async Task<RemoveAssigneeResponse> RemoveMember(int? userId, RemoveAssgineeRequest request)
        {
            var currentUser = await CheckUser(userId);

            var task = await UnitOfWork.taskRepository.FindAsync(request.taskId);

            if (task.HasOwner(currentUser) == false) throw new DirectoryNotFoundException(nameof(task));

            var user = await UnitOfWork.userRepository.FindAsync(request.userId);

            if (task.HasMember(user) == false) throw new ArgumentException(nameof(task));

            try
            {
                await UnitOfWork.BeginTransaction();
                task.RemoveMember(user);
                UnitOfWork.taskRepository.Update(task);
                await UnitOfWork.CommitTransaction();
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
            var members = await UnitOfWork.userRepository.GetAllByTask(task);
            var response = _mapper.Map<RemoveAssigneeResponse>(task);
            response.Members = _mapper.Map<List<UserDTO>>(members);
            return response;
        }
    }
}