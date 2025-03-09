using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Areas.Admin.Models
{
    public class BookModel
    {
     
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public string book_name { get; set; }
        [Required]
        public string isbn { get; set; }
        [Required]
        public string author_name { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public DateTime publication_date { get; set; }
        public List<int> book_categories { get; set; }

    }
 

}
