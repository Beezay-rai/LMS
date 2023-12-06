using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class IssueBookController : ControllerBase
    {
        private readonly IIssueBook _IssueBook;
        public IssueBookController(IIssueBook IssueBook)
        {
            _IssueBook = IssueBook;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllIssueBook()
        {
            var data = await _IssueBook.GetAllIssueBook();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "IssueBookList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetIssueBookById(int id)
        {
            var data = await _IssueBook.GetIssueBookById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "IssueBook fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssueBook(IssueBookViewModel model)
        {
            var data = await _IssueBook.InsertUpdateIssueBook(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created IssueBook" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
        public async Task<IActionResult> EditIssueBook(IssueBookViewModel model)
        {
            var data = await _IssueBook.InsertUpdateIssueBook(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated IssueBook" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteIssueBook(int id)
        {
            var data = await _IssueBook.DeleteIssueBook(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted IssueBook" : "Not Deleted Try Again", Data = data });
        }
        [HttpPost]
        public async Task<IActionResult> ReturnIssuedBook(int id,bool status)
        {
            var data = await _IssueBook.ReturnedIssuedBook(id,status);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated IssueBook" : "Not Deleted Try Again", Data = data });
        }
    }
}
