using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/admin/staff")]
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
        public async Task<IActionResult> GetAllStaff()
        {
            var data = await _Staff.GetAllStaff();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "StaffList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetStaffById(int staffId)
        {
            var data = await _Staff.GetStaffById(staffId);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Staff fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody]StaffModel model)
        {
            var data = await _Staff.InsertUpdateStaff(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Staff" : "Not Created Try Again", Data = data });
        }

        [HttpPut("{staffId}")]
        public async Task<IActionResult> EditStaff(int staffId,[FromBody] StaffModel model)
        {
            var data = await _Staff.InsertUpdateStaff(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Staff" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete("{staffId}")]
        public async Task<IActionResult> DeleteStaff(int staffId)
        {
            var data = await _Staff.DeleteStaff(staffId);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Staff" : "Not Deleted Try Again", Data = data });
        }

    }
}
