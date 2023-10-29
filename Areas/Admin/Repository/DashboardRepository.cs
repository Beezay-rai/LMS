using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LMS.Areas.Admin.Repository
{
    public class DashboardRepository : IDashboard
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardModel> GetDashboardData()
        {
            var data = new DashboardModel()
            {
                BookCount = await _context.Book.Where(x=>x.Deleted==false).CountAsync() ,
                IssuedCount = await _context.IssueBook.Where(x=>x.Deleted==false).CountAsync(),
                //StudentCount = await _context.Student.Where(x=>x.Deleted==false).CountAsync(),
            };

            //var test = await (from bk in _context.Book
            //                  join iss in _context.IssueBook on bk.Id equals iss.BookId
            //                  join cat in _context.Category on bk.CategoryId equals cat.Id
            //                  select new PreferenceCount()
            //                  {
            //                  }

            //                 ).Take(5).ToListAsync();

            foreach (var item in await _context.Category.Where(x=>x.Deleted == false).ToListAsync())
            {
                var count = new PreferenceCount()
                {
                    Id = item.Id,
                    Name = item.Name,
                    //Count = await _context.IssueBook.Where(x => x.Book.CategoryId == item.Id).CountAsync()
                };
                data.PreferenceCountList.Add(count);

            }
            var month = DateTimeFormatInfo.InvariantInfo.MonthNames.Where(d => !string.IsNullOrEmpty(d)).ToList();
            foreach (var monthName in month)
             {
                int monthId = month.IndexOf(monthName )+1;
                var count = new UserActivityCount()
                {
                    MonthName = monthName,
                    PresentCount = _context.IssueBook.Where(x => x.CreatedDate.Month == monthId ).Count(),
                    PreviousCount = _context.IssueBook.Where(x => x.CreatedDate.Day == monthId && x.CreatedDate.Day == (DateTime.Now.Day)).Count(),
                };
                data.UserActivityCountList.Add(count);
            }

            return data;
        }
    }
}
