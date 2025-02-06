using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string first_name { get; set; }
        public string last_name { get; set; }
    
        [Required]
        public int course_id { get; set; }
        [Required]
        public string email_address { get; set; }
        [Required]
        [StringLength(10), MinLength(10)]
        public string phone_number { get; set; }
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [ForeignKey(nameof(course_id))]
        public Course Course { get; set; }


        public bool delete_status { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
        public string deleted_by { get; set; }
        public DateTime deleted_date { get; set; }
    }
}
