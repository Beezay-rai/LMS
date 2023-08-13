using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    //[Bookize]
    [AllowAnonymous]
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
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "BookList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetBookById(int id)
        {
            var data = await _Book.GetBookById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Book fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel model)
        {
            var data = await _Book.InsertUpdateBook(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Book" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
        public async Task<IActionResult> EditBook(BookViewModel model)
        {
            var data = await _Book.InsertUpdateBook(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Book" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var data = await _Book.DeleteBook(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Book" : "Not Deleted Try Again", Data = data });
        }

    }
}
