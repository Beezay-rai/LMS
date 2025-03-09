using LMS.Areas.Admin.Interface;
using LMS.Areas.Admin.Models;
using LMS.Data;
using LMS.Models;
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

        public async Task<BaseApiResponseModel> GetDashboardData()
        {
            var response = new ApiErrorResponseModel<DashboardModel>();
            var data = new DashboardModel()
            {
                BookCount = await _context.Book.Where(x => x.delete_status == false).CountAsync(),
                IssuedCount = await _context.Transaction.Where(x => x.Deleted == false).CountAsync(),
                //StudentCount = await _context.Student.Where(x => x.Deleted == false).CountAsync(),
            };

            //var test = await (from bk in _context.Book
            //                  join tra in _context.BookTransaction on bk.Id equals tra.BookId

            //                  join cat in _context.BookCategoryDetail on bk.CategoryId equals cat.Id
            //                  select new PreferenceCount()
            //                  {
            //                  }

            //                 ).Take(5).ToListAsync();

            foreach (var item in await _context.Category.Where(x => x.delete_status == false).Take(2).ToListAsync())
            {
                var count = new PreferenceCount()
                {
                    Id = item.Id,
                    Name = item.Name,
                    //Count = await _context.Transaction.Where(x => x.Book.CategoryId == item.Id).CountAsync()
                };
                data.PreferenceCountList.Add(count);

            }
            var month = DateTimeFormatInfo.InvariantInfo.MonthNames.Where(d => !string.IsNullOrEmpty(d)).ToList();
            foreach (var monthName in month)
            {
                int monthId = month.IndexOf(monthName) + 1;
                var count = new UserActivityCount()
                {
                    MonthName = monthName,
                    PresentCount = _context.Transaction.Where(x => x.CreatedDate.Month == monthId).Count(),
                    PreviousCount = _context.Transaction.Where(x => x.CreatedDate.Day == monthId && x.CreatedDate.Day == (DateTime.Now.Day)).Count(),
                };
                data.UserActivityCountList.Add(count);
            }

            return response;
        }
    }
}
