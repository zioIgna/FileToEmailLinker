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
    }
}
