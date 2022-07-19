using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Projects.Events
{
    public class DeleteListTaskDomainEvent : BaseDomainEvent
    {
        public DeleteListTaskDomainEvent(ListTask listTask)
        {
            this.listTask = listTask;
        }

        public ListTask listTask { get; set; }
    }
}