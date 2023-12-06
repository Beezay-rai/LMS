using LMS.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Areas.Admin.Models
{
    public class BookGETModel
    {
        public BookGETModel()
        {
            BookCategoryDetailList = new List<BookCategoryDetailGETModel>();
        }

        public int Id { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        public List<BookCategoryDetailGETModel> BookCategoryDetailList { get; set; }


    }
    public class BookModel
    {
        public BookModel()
        {
            BookCategoryDetailList = new List<BookCategoryDetailModel>();
        }

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
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }
        public List<BookCategoryDetailModel> BookCategoryDetailList { get; set; } 

    }
    public class BookCategoryDetailModel
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int CategoryId { get; set; }
     
    }
    public class BookCategoryDetailGETModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
     
    }

}
