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
        private readonly ILogger<AuthenticateController> _logger;
        public AuthenticateController(IAccount account, ILogger<AuthenticateController> logger)
        {
            _logger = logger;
            _account = account;
        }
        [HttpPost]
        [Route("api/signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var result = await _account.SignUp(model);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //_logger.LogInformation("Model Provided : {Model}", model);
            return Ok(await _account.Login(model));
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("api/googleLogin")]
        public async Task<IActionResult> LoginFromGoogle(string token)
        {
            return Ok(await _account.GoogleLogin(token));

        }
        [HttpPost]
        [AllowAnonymous]
        [Route("api/googleSignup")]
        public async Task<IActionResult> SignUpFromGoogle(string token)
        {
            return Ok(await _account.GoogleSignUp(token));

        }

    }
}
