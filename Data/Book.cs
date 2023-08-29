using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public bool? Deleted { get; set; }

        public string Name { get; set; }
       
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
