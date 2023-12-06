using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class BookGETViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName{ get; set; }
        public int CategoryId { get; set; }

    }
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]  
        public int CategoryId { get; set; }
    }
}
