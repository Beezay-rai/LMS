using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Book
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string book_name { get; set; }
        [Required]
        public string isbn { get; set; }
        [Required]
        public string author_name { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime publication_date { get; set; }

        public bool delete_status { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
        public string deleted_by { get; set; }
        public DateTime deleted_date { get; set; }
    }


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
}
