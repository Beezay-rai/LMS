using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaff _Staff;
        public StaffController(IStaff Staff)
        {
            _Staff = Staff;
        }
        [HttpGet]
        public async Task<IEnumerable<StaffViewModel>> GetAllStaff()
        {
            return await _Staff.GetAllStaff();
        }

        [HttpPost]
        public async Task<bool> CreateStaff(StaffViewModel model)
        {
            return await _Staff.CreateStaff(model);
        }

        [HttpPut]
        public async Task<IActionResult> EditStaff(int id)
        {
            return Ok(await _Staff.GetStaffById(id));
        }

        [HttpDelete]
        public async Task<bool> DeleteStaff(int id)
        {
            return await _Staff.DeleteStaff(id);
        }

    }
}
