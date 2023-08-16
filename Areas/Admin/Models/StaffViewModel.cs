
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [StringLength(10)]
        public string Contact { get; set; }
    }
}
