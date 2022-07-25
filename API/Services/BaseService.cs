using API.Extensions;
using AutoMapper;
using Domain.Entities.Projects;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<User> GetCurrentUser()
        {
            try
            {
                var userId = _contextAccessor.HttpContext?.User.Claims.First(i => i.Type == "UserId").Value;
                var user = await _unitOfWork.userRepository.FindAsync(Guid.Parse(userId));
                if (user == null) throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                return user;
            }
            catch
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            }
        }

        public async Task<bool> ProjectAuthorize(Project project)
        {
            var user = await GetCurrentUser();
            if (project.HasOwner(user) || project.HasMember(user)) return true;
            return false;
        }
    }
}