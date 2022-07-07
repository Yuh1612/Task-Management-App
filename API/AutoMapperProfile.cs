using API.DTOs.Projects;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Entities.Users;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AddUserResponse>();
            CreateMap<User, UpdateUserResponse>();
            CreateMap<User, GetOneResponse>();
            CreateMap<Project, ProjectDTO>();
        }
    }
}