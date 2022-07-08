using API.Exceptions;
using Domain.Entities.Users;
using Domain.Interfaces;

namespace API.Services
{
    public class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<User> CheckUser(int? userId)
        {
            if (userId == null) throw new Exception("Something went wrong");
            var user = await UnitOfWork.userRepository.FindAsync(userId);
            if (user == null) throw new UserNotFoundException("User not found");
            return user;
        }
    }
}