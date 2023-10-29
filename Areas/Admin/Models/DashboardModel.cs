namespace LMS.Areas.Admin.Models
{
    public class DashboardModel
    {
        public int Id { get; set; }
        public int BookCount { get; set; }
        public int IssuedCount { get; set; }
        public int StudentCount { get; set; }
        public List<PreferenceCount> PreferenceCountList { get; set; } = new List<PreferenceCount>();
        public List<UserActivityCount> UserActivityCountList { get; set; } = new List<UserActivityCount>();

    }

    public class PreferenceCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class UserActivityCount
    {
        public int Id { get; set; }
        public string MonthName { get; set; }
        public int PresentCount { get; set; }  
        public int PreviousCount { get; set; }  
    }

    
}
