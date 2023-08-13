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


    }
}
