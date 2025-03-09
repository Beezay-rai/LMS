using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }



        public string Remarks { get; set; }



        public bool Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }


    public class BookTransaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TransactionId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public TransactionStatus Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

    }
    public enum TransactionStatus
    {
        Issued,
        Returned,
        Overdue
    }



}
