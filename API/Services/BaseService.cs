using AutoMapper;
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

        public Guid GetCurrentUserId()
        {
            return Guid.Parse(_contextAccessor.HttpContext?.User.Claims.First(i => i.Type == "UserId").Value);
        }
    }
}