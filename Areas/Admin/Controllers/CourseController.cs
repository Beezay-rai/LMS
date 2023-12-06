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
    public class CourseController : ControllerBase
    {
        private readonly ICourse _Course;
        public CourseController(ICourse Course)
        {
            _Course = Course;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var data = await _Course.GetAllCourse();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "CourseList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var data = await _Course.GetCourseById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Course fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseModel model)
        {
            var data = await _Course.InsertUpdateCourse(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Course" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
        public async Task<IActionResult> EditCourse(CourseModel model)
        {
            var data = await _Course.InsertUpdateCourse(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Course" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var data = await _Course.DeleteCourse(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Course" : "Not Deleted Try Again", Data = data });
        }

    }
}
