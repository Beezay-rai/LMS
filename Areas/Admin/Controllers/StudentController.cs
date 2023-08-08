using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _student;
        public StudentController(IStudent Student)
        {
            _student = Student;
        }
        [HttpGet]
        public async Task<IEnumerable<StudentViewModel>> GetAllStudent()
        {
            return await _student.GetAllStudent();
        }

        [HttpPost]
        public async Task<bool> CreateStudent(StudentViewModel model)
        {
            return await _student.CreateStudent(model);
        }


        [HttpPut]
        public async Task<IActionResult> EditStudent(int id)
        {
            return Ok(await _student.GetStudentById(id));
        }

        [HttpDelete]
        public async Task<bool> DeleteStudent(int id)
        {
            return await _student.DeleteStudent(id);
        }


    }
}
