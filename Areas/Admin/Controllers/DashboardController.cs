using LMS.Areas.Admin.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboard ;
        public DashboardController(IDashboard dashboard)
        {
            _dashboard = dashboard;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _dashboard.GetDashboardData();
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Sucessfully fetched Data " : " Error in fetching data", Data = data });
        }
    }
}
