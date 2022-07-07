using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Projects.Events
{
    public class CreateProjectDomainEvent : BaseDomainEvent
    {
        public CreateProjectDomainEvent(User user, Project project)
        {
            this.project = project;
            this.user = user;
        }

        public Project project { get; set; }
        public User user { get; set; }
    }
}