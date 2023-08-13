using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthor _author;
        public AuthorController(IAuthor Author)
        {
            _author = Author;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAuthor()
        {
            var data = await _author.GetAllAuthor();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "AuthorList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var data = await _author.GetAuthorById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Author fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorViewModel model)
         {
            var data = await _author.InsertUpdateAuthor(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Author" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(AuthorViewModel model)
        {
            var data = await _author.InsertUpdateAuthor(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Author" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var data = await _author.DeleteAuthor(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Author" : "Not Deleted Try Again", Data = data });
        }

    }
}
