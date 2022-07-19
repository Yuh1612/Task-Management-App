using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.DomainServices;

namespace Domain.DomainServices
{
    public class UserManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> AddUser(User user)
        {
            try
            {
                user.HashPassWord();
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.userRepository.InsertAsync(user);
                await _unitOfWork.CommitTransaction(false);
                return user;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new ArgumentException();
            }
        }

        public async Task<bool> UpdateUser(User oldUser, User newUser)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                oldUser.Update(newUser.Password, newUser.Name, newUser.Email, newUser.Age, newUser.BirthDay);
                _unitOfWork.userRepository.Update(oldUser);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}