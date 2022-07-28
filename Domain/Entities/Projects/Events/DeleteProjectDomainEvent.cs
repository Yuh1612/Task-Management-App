using Domain.Base;

namespace Domain.Entities.Projects.Events
{
    public class DeleteProjectDomainEvent : BaseDomainEvent
    {
        public DeleteProjectDomainEvent(Project project)
        {
            this.project = project;
        }

        public Project project { get; set; }
    }
}