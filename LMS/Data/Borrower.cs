using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Borrower
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Deleted { get; set; }


    }
}
