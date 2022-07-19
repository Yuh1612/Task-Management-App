using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Tasks
{
    public class TaskMember : Entity
    {
        public TaskMember()
        {
        }

        public TaskMember(Task task, User user, bool isActive)
        {
            Task = task;
            User = user;
            IsActive = isActive;
        }

        public Guid? TaskId { get; set; }

        public virtual Tasks.Task Task { get; set; }

        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsActive { get; set; }
    }
}