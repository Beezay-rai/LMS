using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class CourseModel
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string Credits { get; set; }
        public string Description { get; set; }
    }

    public class POSTCourseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string Credits { get; set; }
        public string Description { get; set; }

    }
}

