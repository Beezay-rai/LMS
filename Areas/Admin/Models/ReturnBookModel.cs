using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Areas.Admin.Models
{
    public class ReturnBookModel
    {
        [SwaggerIgnore]
        public int id { get; set; }
        public int[] book_id { get; set; }
    }
}
