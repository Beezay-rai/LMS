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

<<<<<<< HEAD
        public async Task<List<CategoryModel>> GetAllCategory()
        {
            return await _context.Category.Where(x=>x.IsDeleted==false).Select(x => new CategoryModel()
            {
                Id = x.Id,
                CategoryName = x.Name,
            }).ToListAsync();
        }
        public async Task<CategoryModel> GetCategoryById(int id)
        {
            return await _context.Category.Where(x => x.Id == id && x.IsDeleted == false ).Select(x => new CategoryModel()
            {
                Id = x.Id,
                CategoryName = x.Name,
                
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateCategory(CategoryModel model)
=======
        public async Task<List<CategoryViewModel>> GetAllCategory()
        {
            return await _context.Category.Where(x => x.Deleted == false).Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }
        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            return await _context.Category.Where(x => x.Id == id && x.Deleted == false).Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateCategory(CategoryViewModel model)
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
        {
            try
            {
                if (model.Id > 0)
                {
<<<<<<< HEAD
                    var Category = await _context.Category.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsDeleted == false);
                    if (Category != null)
                    {
                        Category.Name = model.CategoryName;

                        _context.Entry(Category).State = EntityState.Modified;
                    }
=======
                    var Category = await _context.Category.FindAsync(model.Id);
                    if (Category != null)
                    {
                        Category.Name = model.Name;
                        Category.Deleted = false;
                        Category.UpdatedBy = _userId;
                        Category.UpdatedDate = DateTime.UtcNow;
                        _context.Entry(Category).State = EntityState.Modified;
                    }
                    else { return false; }
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
                }
                else
                {
                    Category Category = new Category()
                    {
<<<<<<< HEAD
                        Name = model.CategoryName,

=======
                        Name = model.Name,
                        Deleted = false,
                        CreatedBy = _userId,
                        CreatedDate = DateTime.UtcNow
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
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
<<<<<<< HEAD
                var Category = await _context.Category.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
                if (Category != null)
                {
                    Category.IsDeleted = true;
                    _context.Entry(Category).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();
                return true;

=======
                var Category = await _context.Category.FindAsync(id);
                if (Category != null && Category.Deleted==false)
                {
                    Category.Deleted = true;
                    Category.DeletedBy = _userId;
                    Category.DeletedDate = DateTime.UtcNow;
                    _context.Entry(Category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}
