﻿using FileToEmailLinker.Models.Entities;
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
            var upcomingSchedulations = await dashboardService.GetUpcomingSchedulations();
            var unvisualizedAlerts = await alertService.GetUnvisualizedAlertListAsync();
            DashboardViewModel model = new()
            {
                AlertList = unvisualizedAlerts,
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
    }
}
