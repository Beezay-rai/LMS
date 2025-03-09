using LMS.Interface;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAccount _account;
        public AuthenticateController(IAccount account)
        {
            _account = account;
        }
 
        [HttpPost]
        [Route("api/v1/users")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _account.SignUp(model);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _account.Login(model));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/auth/google")]
        public async Task<IActionResult> LoginFromGoogle(string token)
        {
            return Ok(await _account.GoogleLogin(token));
        }


    }
}
