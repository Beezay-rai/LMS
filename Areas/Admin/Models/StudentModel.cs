using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class StudentModel
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public string first_name { get; set; }
        public string last_name { get; set; }
        [Required]
        public int course_id { get; set; }
        [Required]
        public string email_address { get; set; }
        [Required]
        public string phone_number { get; set; }
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }



    }
}
