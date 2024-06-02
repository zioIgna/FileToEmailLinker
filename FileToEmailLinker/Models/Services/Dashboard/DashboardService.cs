using FileToEmailLinker.Models.Services.Alert;
using FileToEmailLinker.Models.Services.Schedulation;
using FileToEmailLinker.Models.ViewModels;
using FileToEmailLinker.Models.ViewModels.Dashboard;

namespace FileToEmailLinker.Models.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ISchedulationService schedulationService;
        private readonly IConfiguration configuration;
        private readonly IAlertService alertService;

        public DashboardService(ISchedulationService schedulationService, IConfiguration configuration, IAlertService alertService)
        {
            this.schedulationService = schedulationService;
            this.configuration = configuration;
            this.alertService = alertService;
        }

        public async Task<Dictionary<DateOnly, ICollection<Entities.Schedulation>>> GetUpcomingSchedulations()
        {
            var today = DateOnly.FromDateTime(DateTime.Now.Date);
            Dictionary<DateOnly, ICollection<Entities.Schedulation>> schedulationDict = new Dictionary<DateOnly, ICollection<Entities.Schedulation>>();
            int schedulationCount = 0;
            int schedulationLimit = Int32.Parse(configuration["DashboardSchedulationLimit"] ?? "10");
            for (int i = 0; i < 365; i++)
            {
                if (schedulationCount < schedulationLimit)
                {
                    var date = today.AddDays(i);
                    var schedulations = await schedulationService.GetActiveSchedulationsByDateOrWeekDay(date);
                    if (schedulations.Count > 0)
                    {
                        schedulationDict.Add(date, schedulations.OrderBy(sched => sched.Time).ToList());
                        schedulationCount += schedulations.Count;
                    }
                }
            }
                    
            return schedulationDict;
        }

        public async Task<Dictionary<DateOnly, ICollection<Entities.Schedulation>>> GetSchedulationByDate(DateOnly date)
        {
            Dictionary<DateOnly, ICollection<Entities.Schedulation>> schedulationDict = new Dictionary<DateOnly, ICollection<Entities.Schedulation>>();
            var schedulations = await schedulationService.GetActiveSchedulationsByDateOrWeekDay(date);
            schedulationDict.Add(date, schedulations.OrderBy(sched => sched.Time).ToList());
            return schedulationDict;
        }

        public async Task<DashboardViewModel> GetDashboardViewModel()
        {
            DashboardViewModel model = new();
            Dictionary<DateOnly, ICollection<Entities.Schedulation>> upcomingSchedulations = await GetUpcomingSchedulations();
            AlertsListViewModel unvisualizedAlertList = await alertService.GetUnvisualizedAlertListViewModelAsync(1, 10);
            AlertsListViewModel visualizedAlertList = await alertService.GetVisualizedAlertListViewModelAsync(1, 10);
            model.SchedulationGroupList = upcomingSchedulations;
            model.UnvisualizedAlertList = unvisualizedAlertList;
            model.VisualizedAlertList = visualizedAlertList;

            return model;
        }
    }
}
