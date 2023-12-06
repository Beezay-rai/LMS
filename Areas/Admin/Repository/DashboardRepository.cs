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

        public async Task<DashboardViewModel> GetDashboardData()
        {
            var data = new DashboardViewModel()
            {
                BookCount = await _context.Book.Where(x=>x.Deleted==false).CountAsync() ,
<<<<<<< HEAD
                IssuedCount = await _context.Transaction.Where(x=>x.Deleted==false).CountAsync(),
=======
                IssuedCount = await _context.IssueBook.Where(x=>x.Deleted==false).CountAsync(),
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
                StudentCount = await _context.Student.Where(x=>x.Deleted==false).CountAsync(),
            };

            //var test = await (from bk in _context.Book
<<<<<<< HEAD
            //                  join iss in _context.Transaction on bk.Id equals iss.BookId
=======
            //                  join iss in _context.IssueBook on bk.Id equals iss.BookId
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
            //                  join cat in _context.Category on bk.CategoryId equals cat.Id
            //                  select new PreferenceCount()
            //                  {
            //                  }

            //                 ).Take(5).ToListAsync();

<<<<<<< HEAD
            foreach (var item in await _context.Category.Where(x=>x.IsDeleted == false).ToListAsync())
=======
            foreach (var item in await _context.Category.Where(x=>x.Deleted == false).ToListAsync())
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
            {
                var count = new PreferenceCount()
                {
                    Id = item.Id,
                    Name = item.Name,
<<<<<<< HEAD
                    //Count = await _context.Transaction.Where(x => x.Book.CategoryId == item.Id).CountAsync()
=======
                    Count = await _context.IssueBook.Where(x => x.Book.CategoryId == item.Id).CountAsync()
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
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
<<<<<<< HEAD
                    PresentCount = _context.Transaction.Where(x => x.CreatedDate.Month == monthId ).Count(),
                    PreviousCount = _context.Transaction.Where(x => x.CreatedDate.Day == monthId && x.CreatedDate.Day == (DateTime.Now.Day)).Count(),
=======
                    PresentCount = _context.IssueBook.Where(x => x.CreatedDate.Month == monthId ).Count(),
                    PreviousCount = _context.IssueBook.Where(x => x.CreatedDate.Day == monthId && x.CreatedDate.Day == (DateTime.Now.Day)).Count(),
>>>>>>> 67a1c07551f7e83831b7755c71f1dc67cd372c3f
                };
                data.UserActivityCountList.Add(count);
            }

            return data;
        }
    }
}
