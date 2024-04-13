using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Dictionary<DateOnly, ICollection<Schedulation>> SchedulationGroupList { get; set; }
        public ICollection<Alert> AlertList { get; set; }
    }
}
