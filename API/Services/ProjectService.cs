using API.DTOs.Projects;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Interfaces;
using System.Net;

namespace API.Services
{
    public class ProjectService : BaseService
    {
        public ProjectService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
            : base(unitOfWork, contextAccessor, mapper)
        {
        }

        public async Task<List<ProjectDetailDTO>> GetAll()
        {
            var projects = await _unitOfWork.projectRepository.GetAllByUser(GetCurrentUserId());
            return _mapper.Map<List<ProjectDetailDTO>>(projects);
        }

        public async Task<ProjectDTO> GetOne(Guid projectId)
        {
            var project = await _unitOfWork.projectRepository.GetProject(projectId, GetCurrentUser());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            //if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var members = await _unitOfWork.userRepository.GetAllByProject(projectId);
            var response = _mapper.Map<ProjectDTO>(project);
            _mapper.Map(members, response.Members);
            return response;
        }

        public async Task<ProjectDetailDTO> CreateProject(CreateProjectDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var project = new Project(request.Name, request.Description);
                project.AddMember(GetCurrentUserId(), true);
                await _unitOfWork.projectRepository.InsertAsync(project);
                project.AddListTask(new ListTask("Planning"));
                project.AddListTask(new ListTask("To-do"));
                project.AddListTask(new ListTask("Doing"));
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<ProjectDetailDTO>(project);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteProject(Guid Id)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(Id);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            //if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.Delete();
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task UpdateProject(ProjectDetailDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.Id);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            //if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.Update(request.Name, request.Description);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task AddMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            //if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (project.HasMember(member.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.AddMember(member.Id);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task RemoveMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            // (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (!project.HasMember(member.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.RemoveMember(member.Id);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ListTaskDTO> GetOneListTask(Guid Id)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            // if (!await ProjectAuthorize(listTask.Project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            return _mapper.Map<ListTaskDTO>(listTask);
        }

        public async Task CreateListTask(CreateListTaskDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            // if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.AddListTask(_mapper.Map<ListTask>(request));
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task RemoveListTask(Guid Id)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var project = listTask.Project;

            // if (!await ProjectAuthorize(project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            try
            {
                await _unitOfWork.BeginTransaction();
                project.RemoveListTask(listTask);
                _unitOfWork.projectRepository.Update(project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task UpdateListTask(ListTaskDetailDTO request)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(request.Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            // if (!await ProjectAuthorize(listTask.Project)) throw new HttpResponseException(HttpStatusCode.Forbidden);

            try
            {
                await _unitOfWork.BeginTransaction();
                listTask.Update(request.Name, request.Color);
                _unitOfWork.listTaskRepository.Update(listTask);
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