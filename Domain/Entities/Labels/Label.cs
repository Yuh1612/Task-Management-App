using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Labels
{
    public partial class Label : BaseEntity<int>
    {
        public Label()
        {
            Tasks = new HashSet<Tasks.Task>();
        }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Color { get; set; }

        public virtual ICollection<Tasks.Task> Tasks { get; set; }
    }
}