using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Contact { get; set; }
        public bool? Deleted { get; set; }

    }
}
