using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class FacultyRepository : IFaculty
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public FacultyRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<FacultyViewModel>> GetAllFaculty()
        {
            return await _context.Faculty.Where(x => x.Deleted == false).Select(x => new FacultyViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }
        public async Task<FacultyViewModel> GetFacultyById(int id)
        {
            return await _context.Faculty.Where(x => x.Id == id && x.Deleted == false).Select(x => new FacultyViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateFaculty(FacultyViewModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var Faculty = await _context.Faculty.FindAsync(model.Id);
                    if (Faculty != null)
                    {
                        Faculty.Name = model.Name;
                        Faculty.Deleted = false;
                        Faculty.UpdatedBy = _userId;
                        Faculty.UpdatedDate = DateTime.UtcNow;
                        _context.Entry(Faculty).State = EntityState.Modified;
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    Faculty Faculty = new Faculty()
                    {
                        Name = model.Name,
                        Deleted = false,
                        CreatedBy = _userId,
                        CreatedDate = DateTime.UtcNow
                    };
                    await _context.Faculty.AddAsync(Faculty);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteFaculty(int id)
        {
            try
            {
                var Faculty = await _context.Faculty.FindAsync(id);
                if (Faculty != null)
                {
                    Faculty.Deleted = true;
                    Faculty.DeletedBy = _userId;
                    Faculty.DeletedDate = DateTime.UtcNow;
                    _context.Entry(Faculty).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}
