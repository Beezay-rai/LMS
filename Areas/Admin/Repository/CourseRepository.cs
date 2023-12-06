using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.Areas.Admin.Repository
{
    public class CourseRepository : ICourse
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public CourseRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<CourseModel>> GetAllCourse()
        {
            return await _context.Course.Where(x => x.Deleted == false).Select(x => new CourseModel()
            {
                Id = x.Id,
                CourseName = x.CourseName,
                Semester = x.Semester,
                Credits = x.Credits,
                Description = x.Description,
            }).ToListAsync();
        }
        public async Task<CourseModel> GetCourseById(int id)
        {
            return await _context.Course.Where(x => x.Id == id && x.Deleted == false).Select(x => new CourseModel()
            {
                Id = x.Id,
                CourseName = x.CourseName,
                Semester = x.Semester,
                Credits = x.Credits,
                Description = x.Description,
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> InsertUpdateCourse(CourseModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var Course = await _context.Course.FindAsync(model.Id);
                    if (Course != null)
                    {
                        Course.CourseName = model.CourseName;
                        Course.Credits = model.Credits;
                        Course.Semester = model.Semester;
                        Course.Description = model.Description;
                        Course.Deleted = false;
                        Course.UpdatedBy = _userId;
                        Course.UpdatedDate = DateTime.UtcNow;
                        _context.Entry(Course).State = EntityState.Modified;
                    }
                    else { return false; }
                }
                else
                {
                    Course Faculty = new Course()
                    {
                        CourseName = model.CourseName,
                        Credits=model.Credits,
                        Semester= model.Semester,
                        Description= model.Description, 
                        Deleted = false,
                        CreatedBy = _userId,
                        CreatedDate = DateTime.UtcNow
                    };
                    await _context.Course.AddAsync(Faculty);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteCourse(int id)
        {
            try
            {
                var Faculty = await _context.Course.FindAsync(id);
                if (Faculty != null && Faculty.Deleted==false)
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
