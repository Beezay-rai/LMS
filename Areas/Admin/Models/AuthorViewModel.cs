using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
