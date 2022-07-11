using API.DTOs.ListTasks;
using API.DTOs.Tasks;
using AutoMapper;
using Domain.Entities.ListTasks;
using Domain.Interfaces;

namespace API.Services
{
    public class ListTaskService : BaseService
    {
        private readonly IMapper _mapper;

        public ListTaskService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<GetOneListTaskResponse> GetOne(int? userId, GetOneListTaskRequest request)
        {
            var user = await CheckUser(userId);

            var listTask = await UnitOfWork.listTaskRepository.FindAsync(request.Id);

            if (listTask.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(listTask));

            var response = _mapper.Map<GetOneListTaskResponse>(listTask);

            return response;
        }

        public async Task<AddListTaskResponse> AddListTask(int? userId, AddListTaskRequest request)
        {
            var user = await CheckUser(userId);

            var project = await UnitOfWork.projectRepository.FindAsync(request.projectId);

            if (project.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(project));

            try
            {
                await UnitOfWork.BeginTransaction();
                var listTask = new ListTask(user, project, request.Name);
                await UnitOfWork.listTaskRepository.InsertAsync(listTask);
                await UnitOfWork.CommitTransaction();
                var response = _mapper.Map<AddListTaskResponse>(listTask);
                return response;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<UpdateListTaskResponse> UpdateListTask(int? userId, UpdateListTaskRequest request)
        {
            var user = await CheckUser(userId);

            var listTask = await UnitOfWork.listTaskRepository.FindAsync(request.Id);

            if (listTask.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(listTask));

            try
            {
                await UnitOfWork.BeginTransaction();
                listTask.Update(user, request.Name, request.Color);
                UnitOfWork.listTaskRepository.Update(listTask);
                await UnitOfWork.CommitTransaction();
                var response = _mapper.Map<UpdateListTaskResponse>(listTask);
                return response;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}