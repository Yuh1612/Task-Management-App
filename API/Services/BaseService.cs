using API.Extensions;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly IMapper _mapper;
        protected readonly IAuthorizationService _authorizationService;

        public BaseService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper, IAuthorizationService authorizationService)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _authorizationService = authorizationService;
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
    }
}