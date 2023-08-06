using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBook _book;
        public BookController(IBook book)
        {
            _book = book;
        }
        [HttpGet("GetAllBook")]
        public async Task<IActionResult> GetAllBook()
        {
            return Ok(await _book.GetAllBook());
        }

        [HttpPost]
        public async Task<bool> CreateBook(BookViewModel model)
        {
            return await _book.CreateBook(model);
        }

        [HttpPut]
        public async Task<IActionResult> EditBook(int id)
        {
            return Ok(await _book.GetBookById(id));
        }

        [HttpDelete]
        public async Task<bool> DeleteBook(int id)
        {
            return await _book.DeleteBook(id);
        }

    }
}
