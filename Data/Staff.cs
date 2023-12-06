using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
<<<<<<< HEAD
        [Required]
=======
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
<<<<<<< HEAD
        [EmailAddress]
        public string EmailAddress { get; set; }
        [StringLength(10),MinLength(10)]
        public string Contact { get; set; }

=======
        public string EmailAddress { get; set; }
        public string Contact { get; set; }
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
