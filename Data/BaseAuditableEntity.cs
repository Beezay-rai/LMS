using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class BaseAuditableEntity
    {
        public string? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
        public string? deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }
    }
}
