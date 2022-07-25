using API.DTOs.Projects;
using API.DTOs.Tasks;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using Domain.Entities.Users;
using Domain.Histories;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, UserMinDTO>();
            CreateMap<User, UserDetailDTO>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();

            CreateMap<Project, ProjectDTO>();
            CreateMap<Project, ProjectDetailDTO>();
            CreateMap<ProjectDetailDTO, Project>();
            CreateMap<CreateProjectDTO, Project>();

            CreateMap<ListTask, ListTaskDTO>();
            CreateMap<ListTask, ListTaskDetailDTO>();
            CreateMap<ListTaskDetailDTO, ListTask>();
            CreateMap<CreateListTaskDTO, ListTask>();

            CreateMap<Domain.Entities.Tasks.Task, TaskDTO>();
            CreateMap<Domain.Entities.Tasks.Task, TaskDetailDTO>();
            CreateMap<TaskDetailDTO, Domain.Entities.Tasks.Task>();
            CreateMap<CreateTaskDTO, Domain.Entities.Tasks.Task>();
            CreateMap<Todo, TodoDTO>();
            CreateMap<CreateTodoDTO, Todo>();
            CreateMap<Todo, SubTodoDTO>();
            CreateMap<Attachment, AttachmentDTO>();
            CreateMap<Label, LabelDTO>();
        }
    }
}