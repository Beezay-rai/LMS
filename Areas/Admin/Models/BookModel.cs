using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class BookModel
    {

        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string isbn { get; set; }
        [Required]
        public string author_name { get; set; }

        [Required]
        public DateTime publication_date { get; set; }
        public List<int> book_categories { get; set; }

    }


}
