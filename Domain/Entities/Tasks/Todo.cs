using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Tasks
{
    public partial class Todo : Entity
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
        public Guid? ParentId { get; set; }

        [Required]
        public virtual Task Task { get; set; }

        public virtual ICollection<Todo> SubTodos { get; set; }
    }
}