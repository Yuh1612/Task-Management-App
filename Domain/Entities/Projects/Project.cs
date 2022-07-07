using Domain.Base;
using Domain.Entities.ListTasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Projects
{
    public partial class Project : BaseEntity<int>
    {
        public Project()
        {
            this.ProjectMembers = new HashSet<ProjectMember>();
            this.ListTasks = new HashSet<ListTask>();
        }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }

        public virtual ICollection<ListTask> ListTasks { get; set; }
    }
}