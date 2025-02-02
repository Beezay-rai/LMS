﻿using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Areas.Admin.Controllers
{
    [Route("api/v1/admin/category")]
    [ApiController]
    [Authorize]
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
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var data = await _repo.GetCategoryById(categoryId);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryModel model)
        {
            var data = await _repo.AddCategory(model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId, [FromBody] CategoryModel model)
        {
            var data = await _repo.UpdateCategory(categoryId,model);
            return StatusCode((int)data.HttpStatusCode, data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await _repo.DeleteCategory(id);
            return StatusCode((int)data.HttpStatusCode, data);
        }

    }
}
