using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/admin/category")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _repo;
        public CategoryController(ICategory Category)
        {
            _repo = Category;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var data = await _repo.GetAllCategory();
            return Ok(new ApiResponseModel() { Status = data.Any(), Message = data.Any() ? "CategoryList Generated Sucessfully" : "Not Generated Try Again !", Data = data });
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var data = await _repo.GetCategoryById(categoryId);
            return Ok(new ApiResponseModel() { Status = data != null, Message = data != null ? "Category fetched by Id Sucessfully" : "Not Fetched by Id Try Again !", Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(POSTCategoryModel model)
        {
            var data = await _repo.AddCourse(model);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Created Category" : "Not Created Try Again", Data = data });
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId, [FromBody] POSTCategoryModel model)
        {
            var data = await _repo.UpdateCourse(categoryId,model);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Updated Category" : "Not Updated Try Again", Data = data });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await _repo.DeleteCategory(id);
            return Ok(new ApiResponseModel() { Status = data, Message = data ? "Successfully Deleted Category" : "Not Deleted Try Again", Data = data });
        }

    }
}
