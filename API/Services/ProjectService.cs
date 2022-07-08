using API.DTOs.Projects;
using API.DTOs.Users;
using API.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;

namespace API.Services
{
    public class ProjectService : BaseService
    {
        private readonly IMapper _mapper;
        private readonly IProjectDomainService _projectDomainService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IProjectDomainService projectDomainService) : base(unitOfWork)
        {
            _mapper = mapper;
            _projectDomainService = projectDomainService;
        }

        public async Task<GetOneProjectResponse> GetOne(int? userId, GetOneProjectRequest request)
        {
            var user = await CheckUser(userId);
            if (await UnitOfWork.projectMemberRepository.IsProjectExist(user.Id, request.Id) == false) throw new Exception("Project Not Found");

            var project = await UnitOfWork.projectRepository.FindAsync(request.Id);
            if (project == null) throw new Exception("Invalid Input");
            var members = await UnitOfWork.userRepository.GetAllByProject(project);
            var response = _mapper.Map<GetOneProjectResponse>(project);
            response.Members = _mapper.Map<List<UserDTO>>(members);
            return response;
        }

        public async Task<UpdateProjectResponse> UpdateProject(int? userId, UpdateProjectRequest request)
        {
            var user = await CheckUser(userId);
            if (await UnitOfWork.projectMemberRepository.IsProjectExist(user.Id, request.Id) == false) throw new Exception("Project Not Found");
            if (await UnitOfWork.projectMemberRepository.IsProjectExist(user.Id, request.Name) == true) throw new Exception("Exist this Name");

            try
            {
                await UnitOfWork.BeginTransaction();
                var project = await UnitOfWork.projectRepository.FindAsync(request.Id);
                project.Update(user, request.Name, request.Description);
                UnitOfWork.projectRepository.Update(project);
                await UnitOfWork.CommitTransaction();
                var response = _mapper.Map<UpdateProjectResponse>(project);
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