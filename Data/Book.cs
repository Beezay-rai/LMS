using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
<<<<<<< HEAD
        [Required]
        public string BookName { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get;set; }
      
        public bool? Deleted { get; set; }
=======
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public bool? Deleted { get; set; }

        public string Name { get; set; }
       
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
<<<<<<< HEAD


    public class BookCategoryDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
    }
=======
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
}
