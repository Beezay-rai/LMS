using LMS.Areas.Admin.Models;

namespace LMS.Areas.Admin.Interface
{
    public interface IDashboard
    {
        Task<DashboardViewModel> GetDashboardData();
    }
}
