using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/admin/course")]
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
            return StatusCode((int)data.HttpStatusCode, data);
       }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var data = await _Course.GetCourseById(courseId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseModel model)
        {
            var data = await _Course.AddCourse(model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> EditCourse(int courseId, [FromBody] CourseModel model)
        {
            var data = await _Course.UpdateCourse(courseId, model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var data = await _Course.DeleteCourse(courseId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

    }
}
