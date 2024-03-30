using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Services.Schedulation;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace FileToEmailLinker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ISchedulationService schedulationService;

        public DashboardController(ISchedulationService schedulationService)
        {
            this.schedulationService = schedulationService;
        }
        public async Task<IActionResult> Index()
        {
            var today = DateOnly.FromDateTime(DateTime.Now.Date);
            Dictionary<DateOnly,ICollection<Schedulation>> schedulationDict = new Dictionary<DateOnly, ICollection<Schedulation>>();
            int schedulationCount = 0;
            for(int i = 0; i < 365; i++)
            {
                if(schedulationCount < 20)
                {
                    var date = today.AddDays(i);
                    var schedulations = await schedulationService.GetSchedulationsByDateOrWeekDay(date);
                    if(schedulations.Count > 0)
                    {
                        schedulationDict.Add(date, schedulations);
                        schedulationCount += schedulations.Count;
                    }
                }
            }

            return View(schedulationDict);
        }
    }
}
