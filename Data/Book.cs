using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int Total { get; set; }
        public int? Remaining { get; set; }
      


        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }

    public class BookCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }  
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }  
    }
}
