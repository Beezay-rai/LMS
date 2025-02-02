using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/admin/course")]
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
            return Ok(new ApiResponseModel() { Status = data.Any(), Message = data.Any() ? "CourseList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var data = await _Course.GetCourseById(courseId);
            return Ok(new ApiResponseModel() { Status = data != null, Message = data != null ? "Course fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] POSTCourseModel model)
        {
            var data = await _Course.AddCourse(model);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Created Course" : "Not Created Try Again", Data = data });
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> EditCourse(int courseId, [FromBody] POSTCourseModel model)
        {
            var data = await _Course.UpdateCourse(courseId, model);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Updated Course" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var data = await _Course.DeleteCourse(courseId);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Deleted Course" : "Not Deleted Try Again", Data = data });
        }

    }
}
