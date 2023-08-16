using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public int GenderId { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}
