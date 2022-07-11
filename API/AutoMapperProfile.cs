using API.DTOs.ListTasks;
using API.DTOs.Projects;
using API.DTOs.Tasks;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.ListTasks;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using Domain.Entities.Users;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AddUserResponse>();
            CreateMap<User, UpdateUserResponse>();
            CreateMap<User, GetOneUserResponse>();
            CreateMap<User, UserDTO>();

            CreateMap<Project, ProjectDTO>();
            CreateMap<Project, GetOneProjectResponse>();
            CreateMap<Project, UpdateProjectResponse>();
            CreateMap<Project, AddProjectResponse>();
            CreateMap<Project, DTOs.Projects.AddMemberResponse>();
            CreateMap<Project, DTOs.Projects.RemoveMemberResponse>();

            CreateMap<ListTask, ListTaskDTO>();
            CreateMap<ListTask, GetOneListTaskResponse>();
            CreateMap<ListTask, AddListTaskResponse>();
            CreateMap<ListTask, UpdateListTaskResponse>();

            CreateMap<Domain.Entities.Tasks.Task, TaskDTO>();
            CreateMap<Domain.Entities.Tasks.Task, AddTaskResponse>();
            CreateMap<Domain.Entities.Tasks.Task, UpdateTaskResponse>();
            CreateMap<Domain.Entities.Tasks.Task, GetOneTaskResponse>();
            CreateMap<Domain.Entities.Tasks.Task, AddTodoResponse>();
            CreateMap<Domain.Entities.Tasks.Task, AddAttachmentResponse>();
            CreateMap<Domain.Entities.Tasks.Task, RemoveAttachmentResponse>();
            CreateMap<Domain.Entities.Tasks.Task, DTOs.Tasks.AddAssgineeResponse>();
            CreateMap<Domain.Entities.Tasks.Task, DTOs.Tasks.RemoveAssigneeResponse>();

            CreateMap<Todo, TodoDTO>();
            CreateMap<Todo, SubTodoDTO>();

            CreateMap<Attachment, AttachmentDTO>();
        }
    }
}