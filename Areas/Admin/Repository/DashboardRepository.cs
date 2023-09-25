using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Admin.Repository
{
    public class DashboardRepository : IDashboard
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardData()
        {
            var data = new DashboardViewModel()
            {
                BookCount = await _context.Book.Where(x=>x.Deleted==false).CountAsync(),
                IssuedCount = await _context.IssueBook.Where(x=>x.Deleted==false).CountAsync(),
                StudentCount = await _context.Student.Where(x=>x.Deleted==false).CountAsync(),
            };
            foreach (var item in await _context.Category.Where(x=>x.Deleted == false).ToListAsync())
            {
                var count = new PreferenceCount()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Count = await _context.IssueBook.Where(x => x.Book.CategoryId == item.Id).CountAsync()
                };
                data.PreferenceCountList.Add(count);

            }
            return data;
        }
    }
}
