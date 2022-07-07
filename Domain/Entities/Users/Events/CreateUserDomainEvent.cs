using Domain.Base;

namespace Domain.Entities.Users.Events
{
    public class CreateUserDomainEvent : BaseDomainEvent
    {
        public CreateUserDomainEvent(User user)
        {
            this.user = user;
        }

        public User user { get; set; }
    }
}