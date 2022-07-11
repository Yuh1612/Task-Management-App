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
            if (userId == null) throw new KeyNotFoundException(nameof(userId));
            var user = await UnitOfWork.userRepository.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException(nameof(user));
            return user;
        }
    }
}