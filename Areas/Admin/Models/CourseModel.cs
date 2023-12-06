using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string Credits { get; set; }
        public string Description { get; set; }
    }
}

