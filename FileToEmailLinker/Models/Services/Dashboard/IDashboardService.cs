﻿using FileToEmailLinker.Models.ViewModels.Dashboard;

namespace FileToEmailLinker.Models.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<Dictionary<DateOnly, ICollection<Entities.Schedulation>>> GetUpcomingSchedulations();
        Task<Dictionary<DateOnly, ICollection<Entities.Schedulation>>> GetSchedulationByDate(DateOnly date);
        Task<DashboardViewModel> GetDashboardViewModel();
    }
}