using LMS.Areas.Admin.Models;
using LMS.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IDashboard
    {
        Task<BaseApiResponseModel> GetDashboardData();
    }
}
