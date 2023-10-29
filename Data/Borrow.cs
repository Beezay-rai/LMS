using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]  
        public User User { get; set; }
        [Required]
        public int BookId { get; set;}
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        [Required,DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
