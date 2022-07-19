using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Histories
{
    public class History : BaseEntity
    {
        public History()
        {
        }

        public Guid? UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Ref { get; set; }

        public string? RefId { get; set; }

        [Required]
        [StringLength(200)]
        public string Action { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? AffectedColumns { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }
    }
}