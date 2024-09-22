using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public CategoryRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<CategoryModel>> GetAllCategory()
        {
            return await _context.Category.Where(x=>x.IsDeleted==false).Select(x => new CategoryModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.Category.Where(x => x.Id == id && x.IsDeleted == false ).Select(x => new CategoryModel()
            {
                Id = x.Id,
                Name = x.Name,
                
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateCategory(CategoryModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var Category = await _context.Category.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsDeleted == false);
                    if (Category != null)
                    {
                        Category.Name = model.Name;

                        _context.Entry(Category).State = EntityState.Modified;
                    }
                }
                else
                {
                    Category Category = new Category()
                    {
                        Name = model.Name,

                    };
                    await _context.Category.AddAsync(Category);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var Category = await _context.Category.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
                if (Category != null)
                {
                    Category.IsDeleted = true;
                    _context.Entry(Category).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}
