using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool delete_status { get; set; }
    }


}
