using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Tasks
{
    public class TaskMember : Entity
    {
        public TaskMember()
        {
        }

        public TaskMember(Guid taskId, Guid userId, bool isActive)
        {
            TaskId = taskId;
            UserId = userId;
            IsActive = isActive;
        }

        public Guid? TaskId { get; set; }

        public virtual Tasks.Task Task { get; set; }

        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsActive { get; set; }
    }
}