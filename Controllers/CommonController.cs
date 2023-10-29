using LMS.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommon _common;

        public CommonController(ICommon common)
        {
            _common = common;
        }

        [HttpGet]
        public async Task<IActionResult> GetGender()
        {
            var data = await _common.GetGender();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "Generated Sucessfully" :"Not Generated Try Again !!",Data=data});
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _common.GetRoles();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "Generated Sucessfully" :"Not Generated Try Again !!",Data=data});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await _common.GetAllUser();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "Generated Sucessfully" :"Not Generated Try Again !!",Data=data});
        }

    }
}
