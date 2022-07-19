using Domain.Entities.Users;

namespace Domain.Interfaces.DomainServices
{
    public interface IUserManager
    {
        public Task<User?> AddUser(User user);

        public Task<bool> UpdateUser(User oldUser, User newUser);
    }
}