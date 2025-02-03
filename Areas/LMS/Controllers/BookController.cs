using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/book")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBook _Book;
        public BookController(IBook Book)
        {
            _Book = Book;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBook()
        {
            var data = await _Book.GetAllBook();
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var data = await _Book.GetBookById(bookId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookModel model)
        {
            var data = await _Book.AddBook(model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> EditBook(int bookId, [FromBody] BookModel model)
        {
            var data = await _Book.UpdateBook(bookId,model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var data = await _Book.DeleteBook(bookId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

    }
}
