using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/admin/student")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _Student;
        public StudentController(IStudent Student)
        {
            _Student = Student;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            var data = await _Student.GetAllStudent();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "StudentList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var data = await _Student.GetStudentById(studentId);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Student fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody]StudentModel model)
        {
            var data = await _Student.InsertUpdateStudent(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Student" : "Not Created Try Again", Data = data });
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId,[FromBody] StudentModel model)
        {
            var data = await _Student.InsertUpdateStudent(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Student" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var data = await _Student.DeleteStudent(studentId);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Student" : "Not Deleted Try Again", Data = data });
        }

    }
}
