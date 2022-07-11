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
        public Attachment()
        {
        }

        public Attachment(string fileName, string type, string storageUrl)
        {
            FileName = fileName;
            Type = type;
            StorageUrl = storageUrl;
        }

        [Required]
        [StringLength(200)]
        public string FileName { get; set; }

        [Required]
        [StringLength(200)]
        public string Type { get; set; }

        [Required]
        public string StorageUrl { get; set; }

        [Required]
        public virtual Task Task { get; set; }
    }
}