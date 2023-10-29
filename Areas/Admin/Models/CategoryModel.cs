using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required, MinLength(2)]
        public string Name { get; set; }
    }
}
