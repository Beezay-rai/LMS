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
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string EmailAddress { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

    }
    public class StudentModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

     

    }
}
