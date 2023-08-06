using LMS.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }
        [HttpPost]
        [Route("api/SignUp")]
        public async Task<IActionResult> SignUp([FromForm] SignUpModel model)
        {
            var result = await _account.SignUp(model);
            if (result.Succeeded)
            {
                return Ok("Registered Sucessfully !!");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("api/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _account.Login(model);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);

        }
    }
}
