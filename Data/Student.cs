using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
<<<<<<< HEAD
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
        [StringLength(10),MinLength(10)]
        public string PhoneNumber { get; set; }
=======
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int GenderId { get; set; }
        public int FacultyId { get; set; }
        public bool? Deleted { get; set; }

>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }
<<<<<<< HEAD
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }


        public bool? Deleted { get; set; }
=======
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
