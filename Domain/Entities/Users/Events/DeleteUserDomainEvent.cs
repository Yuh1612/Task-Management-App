using Domain.Base;

namespace Domain.Entities.Users.Events
{
    public class DeleteUserDomainEvent : BaseDomainEvent
    {
        public DeleteUserDomainEvent(User user)
        {
            this.user = user;
        }

        public User user { get; set; }
    }
}