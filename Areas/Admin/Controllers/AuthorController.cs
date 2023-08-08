using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthor _author;
        public AuthorController(IAuthor Author)
        {
            _author = Author;
        }
        [HttpGet]
        public async Task<IEnumerable<AuthorViewModel>> GetAllAuthor()
        {
            return await _author.GetAllAuthor();
        }

        [HttpPost]
        public async Task<bool> CreateAuthor(AuthorViewModel model)
        {
            return await _author.CreateAuthor(model);
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(int id)
        {
            return Ok(await _author.GetAuthorById(id));
        }

        [HttpDelete]
        public async Task<bool> DeleteAuthor(int id)
        {
            return await _author.DeleteAuthor(id);
        }

    }
}
