using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tasks
{
    public partial class Todo : BaseEntity<int>
    {
        public Todo()
        {
            this.SubTodos = new HashSet<Todo>();
        }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public bool IsDone { get; set; }

        [ForeignKey("Todo")]
        public int? ParentId { get; set; }

        [Required]
        public Tasks.Task Task { get; set; }

        public ICollection<Todo> SubTodos { get; set; }
    }
}