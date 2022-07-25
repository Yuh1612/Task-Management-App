using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Tasks
{
    public class Attachment : Entity
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