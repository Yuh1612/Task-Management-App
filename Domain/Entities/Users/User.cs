using Domain.Base;
using Domain.Entities.Projects;
using Domain.Entities.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users
{
    public partial class User : Entity
    {
        public User()
        {
            this.ProjectMembers = new HashSet<ProjectMember>();
            this.TaskMembers = new HashSet<TaskMember>();
        }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiredDay { get; set; }

        public Guid TenantId { get; set; }

        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<TaskMember> TaskMembers { get; set; }
    }
}