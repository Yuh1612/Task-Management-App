using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Tasks
{
    public partial class Label : Entity
    {
        public Label()
        {
            Tasks = new HashSet<Task>();
        }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Color { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}