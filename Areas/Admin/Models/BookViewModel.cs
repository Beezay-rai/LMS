namespace LMS.Areas.Admin.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
