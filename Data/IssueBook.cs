using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class IssueBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
        public string Remarks { get; set; }
        public bool ReturnStatus { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }  

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }  
        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
