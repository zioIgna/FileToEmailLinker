using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Services.Dashboard;
using FileToEmailLinker.Models.Services.Schedulation;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace FileToEmailLinker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var upcomingSchedulations = await dashboardService.GetUpcomingSchedulations();

            return View(upcomingSchedulations);
        }

        public async Task<IActionResult> SearchByDate(DateOnly dateSearch)
        {
            if(dateSearch != DateOnly.MinValue)
            {
                var requiredSchedulations = await dashboardService.GetSchedulationByDate(dateSearch);

                return PartialView("/Views/Shared/Dashboard/_DashboardRows.cshtml", requiredSchedulations);
            }
            return Ok(null);
        }

        public async Task<IActionResult> ClearDateSearch()
        {
            var upcomingSchedulations = await dashboardService.GetUpcomingSchedulations();

            return PartialView("/Views/Shared/Dashboard/_DashboardRows.cshtml", upcomingSchedulations);
        }
    }
}
