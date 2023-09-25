namespace LMS.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public int BookCount { get; set; }
        public int IssuedCount { get; set; }
        public int StudentCount { get; set; }
        public List<PreferenceCount> PreferenceCountList { get; set; } = new List<PreferenceCount>();
    }

    public class PreferenceCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    
}
