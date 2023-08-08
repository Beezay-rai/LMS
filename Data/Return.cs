using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Return
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool? Deleted { get; set; }

        public string Remarks { get; set; }
        [ForeignKey(nameof(BorrowerId))]
        public Borrower Borrower { get;set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get;set; }
    }
}
