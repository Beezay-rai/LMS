using System.ComponentModel.DataAnnotations;

namespace LMS.Areas.Admin.Models
{
    public class BookGETModel
    {
        public BookGETModel() 
        {
            BookCategoryGetList = new List<BookCategoryGetModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
       
        public List<BookCategoryGetModel> BookCategoryGetList { get; set; } 

    }
    public class BookModel
    {
        public BookModel()
        {
            BookCategoryList = new List<BookCategoryModel>();
        }

        public int Id { get; set; }

        [Required,MinLength(2)]
        public string Name { get; set; }

        public int Total { get; set; }

        [Required, MinLength(2)]
        public string AuthorName { get; set; }

        public List<BookCategoryModel> BookCategoryList { get; set; }
    }

    public class BookCategoryModel
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
    public class BookCategoryGetModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string CategoryName { get; set; }
    }
}
