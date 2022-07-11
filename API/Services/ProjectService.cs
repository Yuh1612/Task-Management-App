using API.DTOs.Projects;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Interfaces;

namespace API.Services
{
    public class ProjectService : BaseService
    {
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<GetOneProjectResponse> GetOne(int? userId, GetOneProjectRequest request)
        {
            var user = await CheckUser(userId);

            var project = await UnitOfWork.projectRepository.FindAsync(request.Id);

            if (project.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(project));

            var members = await UnitOfWork.userRepository.GetAllByProject(project);

            var response = _mapper.Map<GetOneProjectResponse>(project);
            response.Members = _mapper.Map<List<UserDTO>>(members);

            return response;
        }

        public async Task<AddProjectResponse> AddProject(int? userId, AddProjectRequest request)
        {
            var user = await CheckUser(userId);

            if (user.HasProject(request.Name) == true) throw new ArgumentException(nameof(user));

            try
            {
                await UnitOfWork.BeginTransaction();

                var project = new Project(user, request.Name, request.Description);
                project.AddMember(user, true);
                await UnitOfWork.projectRepository.InsertAsync(project);

                await UnitOfWork.CommitTransaction();

                var response = _mapper.Map<AddProjectResponse>(project);
                return response;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<UpdateProjectResponse> UpdateProject(int? userId, UpdateProjectRequest request)
        {
            var user = await CheckUser(userId);

            var project = await UnitOfWork.projectRepository.FindAsync(request.Id);

            if (project.HasOwner(user) == false) throw new DirectoryNotFoundException(nameof(project));

            if (user.HasProject(request.Name) == true) throw new ArgumentException(nameof(user));

            try
            {
                await UnitOfWork.BeginTransaction();

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

        public async Task<AddMemberResponse> AddMember(int? userId, AddMemberRequest request)
        {
            var currentUser = await CheckUser(userId);

            var currentProject = await UnitOfWork.projectRepository.FindAsync(request.ProjectId);

            if (currentProject.HasOwner(currentUser) == false) throw new DirectoryNotFoundException(nameof(currentProject));

            var user = await UnitOfWork.userRepository.FindAsync(request.UserId);

            if (user == null) throw new KeyNotFoundException(nameof(user));

            if (currentProject.HasMember(user) == true) throw new ArgumentException(nameof(user));

            try
            {
                await UnitOfWork.BeginTransaction();
                currentProject.AddMember(user);
                await UnitOfWork.CommitTransaction();

                var members = await UnitOfWork.userRepository.GetAllByProject(currentProject);
                var response = _mapper.Map<AddMemberResponse>(currentProject);
                response.Members = _mapper.Map<List<UserDTO>>(members);
                return response;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<RemoveMemberResponse> RemoveMember(int? userId, RemoveMemberRequest request)
        {
            var currentUser = await CheckUser(userId);

            var currentProject = await UnitOfWork.projectRepository.FindAsync(request.ProjectId);

            if (currentProject.HasOwner(currentUser) == false) throw new DirectoryNotFoundException(nameof(currentProject));

            var user = await UnitOfWork.userRepository.FindAsync(request.UserId);

            if (user == null) throw new KeyNotFoundException(nameof(user));

            if (currentProject.HasMember(user) == false) throw new DirectoryNotFoundException(nameof(user));

            try
            {
                await UnitOfWork.BeginTransaction();
                currentProject.RemoveMember(user);
                await UnitOfWork.CommitTransaction();

                var members = await UnitOfWork.userRepository.GetAllByProject(currentProject);
                var response = _mapper.Map<RemoveMemberResponse>(currentProject);
                response.Members = _mapper.Map<List<UserDTO>>(members);
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