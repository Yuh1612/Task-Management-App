using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Projects
{
    public partial class ListTask : Entity
    {
        public ListTask()
        {
            this.Tasks = new HashSet<Tasks.Task>();
        }

        public ListTask(string name, string? color = null)
        {
            Name = name;
            Color = color;
        }

        public void Update(string? name, string? color)
        {
            Name = name ?? Name;
            Color = color ?? Color;
        }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Color { get; set; }

        [Required]
        public virtual Project Project { get; set; }

        public virtual ICollection<Tasks.Task> Tasks { get; set; }
    }
}