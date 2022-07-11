using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Tasks
{
    public class TaskMember : BaseEntity<int>
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

        public int? TaskId { get; set; }

        public virtual Tasks.Task Task { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsActive { get; set; }
    }
}