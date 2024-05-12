using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Models.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public Dictionary<DateOnly, ICollection<Schedulation>> SchedulationGroupList { get; set; }
        public AlertsListViewModel UnvisualizedAlertList { get; set; }
        public AlertsListViewModel VisualizedAlertList { get; set; }
    }
}
