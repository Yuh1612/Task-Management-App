using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Tasks
{
    public class Attachment : BaseEntity<int>
    {
        [Required]
        [StringLength(200)]
        public string FileName { get; set; }

        [Required]
        [StringLength(200)]
        public string Type { get; set; }

        [Required]
        public string StorageUrl { get; set; }

        [Required]
        public Tasks.Task Task { get; set; }
    }
}