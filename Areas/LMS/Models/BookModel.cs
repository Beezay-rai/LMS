using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LMS.Areas.Admin.Models
{
    public class BookModel
    {
        public BookModel()
        {
            BookCategories = new List<BookCategoryModel>();
        }
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        public List<BookCategoryModel> BookCategories { get; set; }

    }
    public class BookCategoryModel
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        [SwaggerIgnore]
        public int BookId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [SwaggerIgnore]
        public string  CategoryName { get; set; }

    }

}
