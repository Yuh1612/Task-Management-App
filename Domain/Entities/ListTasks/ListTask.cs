using Domain.Base;
using Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ListTasks
{
    public partial class ListTask : BaseEntity<int>
    {
        public ListTask()
        {
            this.Tasks = new HashSet<Tasks.Task>();
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