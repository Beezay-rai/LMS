using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.LMS.Controllers
{
    [Route("api/v1/rent-book")]
    [ApiController]
    [Authorize]
    public class RentBookController : ControllerBase
    {
        [HttpGet("{studentId}")]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpPost("{studentId}")]
        public IActionResult RentBooks(RentBookModel model)
        {
            return Ok();
        }
    }
}
