using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class BookCategoryModel
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
