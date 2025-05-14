using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/rent-book")]
    [ApiController]
    [Authorize]
    public class RentBookController : ControllerBase
    {
        private readonly IRentBookRepository _repo;
        public RentBookController(IRentBookRepository RentBook)
        {
            _repo = RentBook;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRentBook()
        {
            var data = await _repo.GetAllRentBook();
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpGet("{RentBookId}")]
        public async Task<IActionResult> GetRentBookById(int RentBookId)
        {
            var data = await _repo.GetRentBookById(RentBookId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentBook(RentBookModel model)
        {
            var data = await _repo.AddRentBook(model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPut("{RentBookId}")]
        public async Task<IActionResult> EditRentBook(int RentBookId, [FromBody] RentBookModel model)
        {
            var data = await _repo.UpdateRentBook(RentBookId, model);

            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpDelete("{RentBookId}")]
        public async Task<IActionResult> DeleteRentBook(int RentBookId)
        {
            var data = await _repo.DeleteRentBook(RentBookId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

    }
}
