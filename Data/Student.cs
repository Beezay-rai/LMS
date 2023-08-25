using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int GenderId { get; set; }
        public int FacultyId { get; set; }
        public bool? Deleted { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
