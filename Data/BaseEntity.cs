using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public abstract class BaseEntity
    {
        public bool Deleted { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
} 