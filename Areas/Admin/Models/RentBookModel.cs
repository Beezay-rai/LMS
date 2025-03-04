using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Areas.Admin.Models
{
    public class RentBookModel
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        public string student_id { get;set; }

        public List<RentBookDetailModel> rent_book { get; set; }
    }

    public class RentBookDetailModel
    {
        public int book_id { get; set; }
        public DateTime return_date { get; set; }
        public bool return_status { get; set; }
    }
  
}
