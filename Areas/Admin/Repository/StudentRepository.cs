//using LMS.Areas.Admin.Interface;
//using LMS.Areas.Admin.Models;
//using LMS.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace LMS.Areas.Admin.Repository
//{
//    public class StudentRepository : IStudent
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IHttpContextAccessor _contextAccessor;
//        private readonly string _userId;

//        public StudentRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
//        {
//            _context = context;
//            _contextAccessor = contextAccessor;
//            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//        }
//        public async Task<List<StudentModel>> GetAllStudent()
//        {
//            return await _context.Student.Where(x => x.Deleted == false).Select(x => new StudentModel()
//            {
//                Id = x.Id,
//                FirstName = x.FirstName,
//                LastName = x.LastName,
//                BirthDate = x.BirthDate,
//                GenderId = x.GenderId,
//                FacultyId = x.FacultyId,
                
//            }).ToListAsync();
//        }
//        public async Task<StudentModel> GetStudentById(int id)
//        {
//            return await _context.Student.Where(x => x.Id == id && x.Deleted == false).Select(x => new StudentModel()
//            {
//                Id = x.Id,
//                FirstName = x.FirstName,
//                LastName = x.LastName,
//                BirthDate = x.BirthDate,
//                GenderId = x.GenderId,
//                FacultyId=x.FacultyId
//            }).FirstOrDefaultAsync();
//        }
//        public async Task<bool> InsertUpdateStudent(StudentModel model)
//        {
//            try
//            {
//                if (model.Id > 0)
//                {
//                    var student = await _context.Student.Where(x => x.Id == model.Id && x.Deleted == false).FirstOrDefaultAsync();
//                    if (student != null)
//                    {
//                        student.FirstName = model.FirstName;
//                        student.LastName = model.LastName;
//                        student.BirthDate = model.BirthDate;
//                        student.GenderId = model.GenderId;
//                        student.FacultyId = model.FacultyId;
//                        student.UpdatedDate = DateTime.UtcNow;
//                        student.UpdatedBy = _userId;

//                        _context.Entry(student).State = EntityState.Modified;
//                    }
//                    else { return false; }
//                }
//                else
//                {
//                    Student student = new Student()
//                    {
//                        FirstName = model.FirstName,
//                        LastName = model.LastName,
//                        BirthDate = model.BirthDate,
//                        GenderId = model.GenderId,
//                        FacultyId = model.FacultyId,
//                        CreatedBy = _userId,
//                        CreatedDate = DateTime.UtcNow,
//                        Deleted = false
//                    };
//                    await _context.Student.AddAsync(student);
//                }
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//        public async Task<bool> DeleteStudent(int id)
//        {
//            var data = await _context.Student.Where(x => x.Id == id && x.Deleted == false).FirstOrDefaultAsync();
//            if (data != null && data.Deleted==false)
//            {
//                data.Deleted = true;
//                data.DeletedDate = DateTime.UtcNow;
//                data.DeletedBy = _userId;
//                _context.Entry(data).State = EntityState.Modified;
//                await _context.SaveChangesAsync();
//                return true;

//            }
//            else { return false; }
//        }

//    }
//}
