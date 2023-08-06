using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public bool? Deleted { get; set; }

        [Required]
        public string Name { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
