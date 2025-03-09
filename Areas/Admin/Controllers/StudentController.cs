using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        public StudentController(IStudentRepository Student)
        {
            _repo = Student;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            var data = await _repo.GetAllStudent();
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var data = await _repo.GetStudentById(studentId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentModel model)
        {
            var data = await _repo.AddStudent(model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId, [FromBody] StudentModel model)
        {
            var data = await _repo.UpdateStudent(studentId, model);

            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var data = await _repo.DeleteStudent(studentId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

    }
}
