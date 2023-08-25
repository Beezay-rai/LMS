using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    //[Facultyize]
    [AllowAnonymous]
    public class FacultyController : ControllerBase
    {
        private readonly IFaculty _Faculty;
        public FacultyController(IFaculty Faculty)
        {
            _Faculty = Faculty;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFaculty()
        {
            var data = await _Faculty.GetAllFaculty();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "FacultyList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetFacultyById(int id)
        {
            var data = await _Faculty.GetFacultyById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Faculty fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateFaculty(FacultyViewModel model)
        {
            var data = await _Faculty.InsertUpdateFaculty(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Faculty" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
        public async Task<IActionResult> EditFaculty(FacultyViewModel model)
        {
            var data = await _Faculty.InsertUpdateFaculty(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Faculty" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            var data = await _Faculty.DeleteFaculty(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Faculty" : "Not Deleted Try Again", Data = data });
        }

    }
}
