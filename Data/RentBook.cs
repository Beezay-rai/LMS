using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class RentBook
    {
        [Key]
        public int id { get; set; }
        public int student_id { get; set; }
        public bool deleted { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
        public string deleted_by { get; set; }
        public DateTime? deleted_date { get; set; }
    }

    public class RentBookDetail
    {
        [Key]
        public int id { get; set; }
        public int book_id { get; set; }
        public DateTime return_date { get; set; }
        public int rent_book_id { get; set; }
    }
}
