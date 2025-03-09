using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/return-book/{rentBookId}")]
    [Authorize]
    [ApiController]
    public class ReturnBookController : ControllerBase
    {
        private readonly IRentBookRepository _repo;

        public ReturnBookController(IRentBookRepository repo)
        {
            _repo = repo;
        }


        [HttpPost]
        public async Task<IActionResult> ReturnBook(int rentBookId, [FromBody] ReturnBookModel model)
        {
            var data = await _repo.ReturnRentBook(rentBookId, model.book_id);
            return StatusCode((int)data.HttpStatusCode, data);
        }


        public class ReturnBookModel
        {
            public int[] book_id { get; set; }
        }

    }

   
}
