using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Borrower
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime IssuedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public int IssuedBy { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
