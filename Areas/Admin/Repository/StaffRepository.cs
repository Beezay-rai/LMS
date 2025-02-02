//using LMS.Areas.Admin.Interface;
//using LMS.Areas.Admin.Models;
//using LMS.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace LMS.Areas.Admin.Repository
//{
//    public class StaffRepository : IStaff
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IHttpContextAccessor _contextAccessor;
//        private readonly string _userId;

//        public StaffRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
//        {
//            _context = context;
//            _contextAccessor = contextAccessor;
//            _userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//        }

//        #region Staff
//        public async Task<List<StaffModel>> GetAllStaff()
//        {
//            return await _context.Staff.Where(x => x.Deleted == false).Select(x => new StaffModel()
//            {
//                Id = x.Id,
//                FirstName = x.FirstName,
//                LastName = x.LastName,
//                BirthDate = x.BirthDate,
//                Contact = x.Contact,
//                EmailAddress = x.EmailAddress
//            }).ToListAsync();
//        }

//        public async Task<StaffModel> GetStaffById(int id)
//        {
//            return await _context.Staff.Where(x => x.Id == id && x.Deleted == false).Select(x => new StaffModel()
//            {
//                Id = x.Id,
//                FirstName = x.FirstName,
//                LastName = x.LastName,
//                BirthDate = x.BirthDate,
//                Contact = x.Contact,
//                EmailAddress = x.EmailAddress
//            }).FirstOrDefaultAsync() ?? new StaffModel();
//        }
//        public async Task<bool> InsertUpdateStaff(StaffModel model)
//        {
//            try
//            {
//                if (model.Id > 0)
//                {
//                    var staff = await _context.Staff.FindAsync(model.Id);
//                    if (staff != null)
//                    {
//                        staff.FirstName = model.FirstName;
//                        staff.LastName = model.LastName;
//                        staff.BirthDate = model.BirthDate;
//                        staff.Contact = model.Contact;
//                        staff.Deleted = false;
//                        staff.UpdatedDate = DateTime.UtcNow;
//                        staff.UpdatedBy = _userId;
//                        _context.Entry(staff).State = EntityState.Modified;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                else
//                {
//                    Staff staff = new Staff()
//                    {
//                        FirstName = model.FirstName,
//                        LastName = model.LastName,
//                        BirthDate = model.BirthDate,
//                        Contact = model.Contact,
//                        EmailAddress = model.EmailAddress,
//                        CreatedBy = _userId,
//                        CreatedDate = DateTime.UtcNow,
//                    };
//                    await _context.Staff.AddAsync(staff);
//                }
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception Ex)
//            {
//                return false;
//            }
//        }

//        public async Task<bool> DeleteStaff(int id)
//        {
//            try
//            {
//                var staff = await _context.Staff.FindAsync(id);
//                if (staff != null && staff.Deleted == false)
//                {
//                    staff.Deleted = true;
//                    staff.DeletedDate = DateTime.UtcNow;
//                    staff.DeletedBy = _userId;
//                    _context.Entry(staff).State = EntityState.Modified;
//                    await _context.SaveChangesAsync();
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//        #endregion



//    }
//}
