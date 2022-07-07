using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Histories
{
    public class History : BaseEntity<int>
    {
        [Required]
        [StringLength(200)]
        public string Ref { get; set; }

        public int? RefId { get; set; }

        [Required]
        [StringLength(200)]
        public string Action { get; set; }

        public string? Message { get; set; }

        public string? contentJson { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [Required]
        public int CreateById { get; set; }
    }
}