using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public bool? Deleted { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }
    }
}
