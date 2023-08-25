using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class IssueBookGETViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int StudentId { get; set; }
        public string StudentFullName{ get; set; }
        [DataType(DataType.Date)]
        public DateTime IssuedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
        public bool ReturnStatus { get; set; }
    }
    public class IssueBookViewModel
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public string Remarks { get; set; }
    }
}
