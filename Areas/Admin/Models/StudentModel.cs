using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class StudentGETModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
    public class StudentModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int FacultyId { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

    }
}
