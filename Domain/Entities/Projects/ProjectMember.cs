using Domain.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Projects
{
    public class ProjectMember : BaseEntity<int>
    {
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsCreated { get; set; }
    }
}