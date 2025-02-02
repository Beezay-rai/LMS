using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Areas.Admin.Models
{
  
    public class CategoryModel
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
