using LMS.Data;
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class TransactionGETModel
    {
        public TransactionGETModel()
        {
            BookTransactionList = new List<BookTransactionGETModel>();
        }
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime IssuedDate { get; set; }
        public string CreatedBy { get; set; }



        public string Remarks { get; set; }

        public List<BookTransactionGETModel> BookTransactionList { get; set; }
    }
    public class TransactionModel
    {
        public TransactionModel()
        {
            BookTransactionList = new List<BookTransactionModel>();
        }
        public int Id { get; set; }


        public string Remarks { get; set; }
        public List<BookTransactionModel> BookTransactionList { get; set; }


    }
    public class BookTransactionGETModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public TransactionStatus Status { get; set; }
        public string StatusName { get; set; }
    }
    public class BookTransactionModel
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
    }
}
