using API.DTOs.Projects;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace API.Services
{
    public class ProjectService : BaseService
    {
        private readonly IProjectManager _projectManager;

        public ProjectService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper,
            IAuthorizationService authorizationService, IProjectManager projectManager)
            : base(unitOfWork, contextAccessor, mapper, authorizationService)
        {
            _projectManager = projectManager;
        }

        public async Task<ProjectDTO> GetOne(Guid Id)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(Id);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var members = await _unitOfWork.userRepository.GetAllByProject(project);
            var response = _mapper.Map<ProjectDTO>(project);
            _mapper.Map(members, response.Members);
            return response;
        }

        public async Task<ProjectDetailDTO> CreateProject(CreateProjectDTO request)
        {
            var user = await GetCurrentUser();
            var project = await _projectManager.CreateProject(user, _mapper.Map<Project>(request));
            return _mapper.Map<ProjectDetailDTO>(project);
        }

        public async Task DeleteProject(Guid Id)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(Id);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);
            if (!await _projectManager.DeleteProject(project)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task UpdateProject(ProjectDetailDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.Id);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _projectManager.UpdateProject(project, _mapper.Map<Project>(request))) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task AddMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (project.HasMember(member)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _projectManager.AddMember(project, member)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task RemoveMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (!project.HasMember(member)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!await _projectManager.RemoveMember(project, member)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task<ListTaskDTO> GetOneListTask(Guid Id)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, listTask.Project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            return _mapper.Map<ListTaskDTO>(listTask);
        }

        public async Task CreateListTask(CreateListTaskDTO request)
        {
            var project = await _unitOfWork.projectRepository.FindAsync(request.projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _projectManager.CreateListTask(project, _mapper.Map<ListTask>(request))) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task RemoveListTask(Guid Id)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, listTask.Project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _projectManager.RemoveListTask(listTask.Project, listTask)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public async Task UpdateListTask(ListTaskDetailDTO request)
        {
            var listTask = await _unitOfWork.listTaskRepository.FindAsync(request.Id);
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var authorizationResult = await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, listTask.Project, "HasUser");
            if (!authorizationResult.Succeeded) throw new HttpResponseException(HttpStatusCode.Forbidden);

            if (!await _projectManager.UpdateListTask(listTask, _mapper.Map<ListTask>(request))) throw new HttpResponseException(HttpStatusCode.BadRequest); ;
        }
    }
}