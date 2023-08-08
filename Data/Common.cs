using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Gender
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Deleted { get; set; }

    }

    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool? Deleted { get; set; }
    }

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Deleted { get; set; }

    }


}
