using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Users.Events;

namespace Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(Guid id, string userName, string? email)
        {
            Id = id;
            UserName = userName;
            Email = email;
            ProjectMembers = new HashSet<ProjectMember>();
        }

        public void AddCreateUserDomainEvent()
        {
            var newEvent = new CreateUserDomainEvent(this);
            AddEvent(newEvent);
        }
    }
}