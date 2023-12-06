using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/Admin/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _Category;
        public CategoryController(ICategory Category)
        {
            _Category = Category;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var data = await _Category.GetAllCategory();
            return Ok(new ApiResponse() { Status = data.Any(), Message = data.Any() ? "CategoryList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var data = await _Category.GetCategoryById(id);
            return Ok(new ApiResponse() { Status = data != null, Message = data != null ? "Category fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
<<<<<<< HEAD
        public async Task<IActionResult> CreateCategory(CategoryModel model)
=======
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        {
            var data = await _Category.InsertUpdateCategory(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Created Category" : "Not Created Try Again", Data = data });
        }

        [HttpPut]
<<<<<<< HEAD
        public async Task<IActionResult> EditCategory(CategoryModel model)
=======
        public async Task<IActionResult> EditCategory(CategoryViewModel model)
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        {
            var data = await _Category.InsertUpdateCategory(model);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Updated Category" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await _Category.DeleteCategory(id);
            return Ok(new ApiResponse() { Status = data, Message = data ? "Successfully Deleted Category" : "Not Deleted Try Again", Data = data });
        }

    }
}
