using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class StaffRepository : IStaff
    {
        private readonly ApplicationDbContext _context;

        public StaffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StaffViewModel>> GetAllStaff()
        {
            return await _context.Staff.Select(x => new StaffViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Contact = x.Contact,
                EmailAddress = x.EmailAddress
            }).ToListAsync();
        }

        public async Task<StaffViewModel> GetStaffById(int id)
        {
            return await _context.Staff.Where(x=>x.Id== id).Select(x=> new StaffViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Contact = x.Contact,
                EmailAddress = x.EmailAddress
            }).FirstOrDefaultAsync() ?? new StaffViewModel();
        }

        public async Task<bool> EditStaff(StaffViewModel model)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(model.Id);
                if (staff != null)
                {
                    staff.FirstName = model.FirstName;
                    staff.LastName = model.LastName;
                    staff.BirthDate = model.BirthDate;
                    staff.Contact = model.Contact;
                    _context.Entry(staff).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> CreateStaff(StaffViewModel model)
        {
            try
            {
                Staff staff = new Staff()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.BirthDate,
                    Contact = model.Contact,
                    EmailAddress = model.EmailAddress
                };
                await _context.Staff.AddAsync(staff);
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception Ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteStaff(int id)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(id);
                if (staff != null)
                {
                    staff.Deleted = true;
                    _context.Entry(staff).State = EntityState.Modified;
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
