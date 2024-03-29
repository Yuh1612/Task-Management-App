﻿using API.DTOs.Projects;
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
            var project = await _unitOfWork.projectRepository.GetProject(projectId, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

            var response = _mapper.Map<ProjectDTO>(project);
            _mapper.Map(project.ProjectMembers.Select(s => s.User), response.Members);
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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task DeleteProject(Guid projectId)
        {
            var project = await _unitOfWork.projectRepository.GetProject(projectId, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task UpdateProject(ProjectDetailDTO request)
        {
            var project = await _unitOfWork.projectRepository.GetProject(request.Id, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task AddMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.GetProject(request.projectId, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");
            if (project.HasMember(member.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest, "User is not a member in this project!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task RemoveMember(ProjectMemberDTO request)
        {
            var project = await _unitOfWork.projectRepository.GetProject(request.projectId, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

            var member = await _unitOfWork.userRepository.FindAsync(request.userId);
            if (member == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");
            if (!project.HasMember(member.Id)) throw new HttpResponseException(HttpStatusCode.BadRequest, "User is not a member in this project!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task<ListTaskDTO> GetOneListTask(Guid listTaskId)
        {
            var listTask = await _unitOfWork.listTaskRepository.GetListTask(listTaskId, GetCurrentUserId());
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Listtask is not found!");

            return _mapper.Map<ListTaskDTO>(listTask);
        }

        public async Task CreateListTask(CreateListTaskDTO request)
        {
            var project = await _unitOfWork.projectRepository.GetProject(request.projectId, GetCurrentUserId());
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Project is not found!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task RemoveListTask(Guid listTaskId)
        {
            var listTask = await _unitOfWork.listTaskRepository.GetListTask(listTaskId, GetCurrentUserId());
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Listtask is not found!");

            try
            {
                await _unitOfWork.BeginTransaction();
                listTask.Project.RemoveListTask(listTask);
                _unitOfWork.projectRepository.Update(listTask.Project);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task UpdateListTask(ListTaskDetailDTO request)
        {
            var listTask = await _unitOfWork.listTaskRepository.GetListTask(request.Id, GetCurrentUserId());
            if (listTask == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Listtask is not found!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }
    }
}