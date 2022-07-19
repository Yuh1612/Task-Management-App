using Domain.Base;
using Domain.Entities.Projects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Tasks
{
    public partial class Task : Entity
    {
        public Task()
        {
            this.Todos = new HashSet<Todo>();
            this.TaskMembers = new HashSet<TaskMember>();
            this.Attachments = new HashSet<Attachment>();
            this.Labels = new HashSet<Label>();
        }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public virtual ListTask ListTask { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }

        public virtual ICollection<TaskMember> TaskMembers { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }
}