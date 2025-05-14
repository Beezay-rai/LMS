using LMS.Areas.Admin.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;
        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("api/v1/users")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _repo.SignUpUser(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/v1/users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _repo.GetAllUser();
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
