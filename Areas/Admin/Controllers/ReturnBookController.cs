using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/return-book")]
    [Authorize]
    [ApiController]
    public class ReturnBookController : ControllerBase
    {
        [HttpGet("{studentId}")]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost("{studentId}")]
        public IActionResult ReturnBook(ReturnBookModel model )
        {
            return Ok();
        }

    }

    public class ReturnBookModel
    {
        public int[] book_id { get; set; }
    }
}
