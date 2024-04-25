using FileToEmailLinker.Models.Services.Schedulation;

namespace FileToEmailLinker.Models.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ISchedulationService schedulationService;
        private readonly IConfiguration configuration;

        public DashboardService(ISchedulationService schedulationService, IConfiguration configuration)
        {
            this.schedulationService = schedulationService;
            this.configuration = configuration;
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
    }
}
