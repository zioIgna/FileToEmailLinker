using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Services.Alert;
using FileToEmailLinker.Models.Services.Dashboard;
using FileToEmailLinker.Models.Services.Schedulation;
using FileToEmailLinker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;

namespace FileToEmailLinker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;
        private readonly IAlertService alertService;

        public DashboardController(IDashboardService dashboardService, IAlertService alertService)
        {
            this.dashboardService = dashboardService;
            this.alertService = alertService;
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<DateOnly, ICollection<Schedulation>> upcomingSchedulations = await dashboardService.GetUpcomingSchedulations();
            ICollection<Alert>? unvisualizedAlerts = await alertService.GetUnvisualizedAlertListAsync();
            ICollection<Alert>? visualizedAlerts = await alertService.GetVisualizedAlertListAsync();
            DashboardViewModel model = new()
            {
                UnvisualizedAlertList = unvisualizedAlerts,
                VisualizedAlertList = visualizedAlerts,
                SchedulationGroupList = upcomingSchedulations
            };

            return View(model);
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

        public async Task<IActionResult> CheckAlert(int id)
        {
            await alertService.CheckAlertAsync(id);
            ICollection<Alert> alerts = await alertService.GetUnvisualizedAlertListAsync();
            //UpdateBadgeCount(alerts.Count);

            return PartialView("Dashboard/_SegnalazioniRows", alerts);
        }

        public async Task<IActionResult> GetVisualizedAlerts()
        {
            ICollection<Alert> alerts = await alertService.GetVisualizedAlertListAsync();

            return PartialView("Dashboard/_VisualizedSegnalazioniRows", alerts);
        }


        //public async Task<IActionResult> UpdateBadgeCount()
        //{
        //    ICollection<Alert> alerts = await alertService.GetUnvisualizedAlertListAsync();

        //    return PartialView("Dashboard/_SegnalazioniBadge", alerts.Count());
        //}
    }
}
