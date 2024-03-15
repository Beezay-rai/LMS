using Google.Apis.Auth;
using LMS.Interface;
using LMS.Models;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        [Route("api/SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _account.SignUp(model);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return Ok(await _account.Login(model));
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("api/GoogleLogin")]
        public async Task<IActionResult> LoginFromGoogle(string token)
        {
            return Ok(await _account.GoogleLogin(token));

        }
        [HttpPost]
        [AllowAnonymous]
        [Route("api/GoogleSignup")]
        public async Task<IActionResult> SignUpFromGoogle(string token)
        {
            return Ok(await _account.GoogleSignUp(token));

        }

    }
}
