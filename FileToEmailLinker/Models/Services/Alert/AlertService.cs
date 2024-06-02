using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.ViewModels;
using FileToEmailLinker.Models.ViewModels.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.Alert
{
    public class AlertService : IAlertService
    {
        private readonly FileToEmailLinkerContext context;

        public AlertService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Entities.Alert>> GetUnvisualizedAlertListAsync()
        {
            IQueryable<Entities.Alert> query = context.Alert
                .Where(al => !al.Visualized && al.AlertSeverity <= Enums.AlertSeverity.Warning)
                .OrderByDescending(al => al.DateTime);

            return await query.ToListAsync();
        }

        public async Task<ICollection<Entities.Alert>> GetVisualizedAlertListAsync()
        {
            IQueryable<Entities.Alert> query = context.Alert
                .Where(al => al.Visualized && al.AlertSeverity <= Enums.AlertSeverity.Warning)
                .OrderByDescending(al => al.DateTime);

            return await query.ToListAsync();
        }

        public async Task<AlertsListViewModel> GetUnvisualizedAlertListViewModelAsync(int page, int limit)
        {
            IQueryable<Entities.Alert> queryLinq = GetUnvisualizedAlertsQuery();
            return await GenerateListViewModel(page, limit, queryLinq, "GetUnvisualizedAlertListNthPage", "unvisualizedAlertTable");
        }

        public async Task<AlertsListViewModel> GetVisualizedAlertListViewModelAsync(int page, int limit)
        {
            IQueryable<Entities.Alert> queryLinq = GetVisualizedAlertsQuery();
            return await GenerateListViewModel(page, limit, queryLinq, "GetVisualizedAlertListNthPage", "visualizedAlertTable");
        }

        private static async Task<AlertsListViewModel> GenerateListViewModel(int page, int limit, IQueryable<Entities.Alert> queryLinq, string action, string targetId)
        {
            ListViewModel<Entities.Alert> listViewModel = new ListViewModel<Entities.Alert>();
            int realPage = Math.Max(1, page);
            int realLimit = Math.Max(1, limit);
            int offset = (realPage - 1) * realLimit;

            List<Entities.Alert> alerts = await queryLinq
                .Skip(offset)
                .Take(realLimit)
                .ToListAsync();

            int totalCount = await queryLinq.CountAsync();
            listViewModel.Results = alerts;
            listViewModel.TotalCount = totalCount;

            AlertsListViewModel alertsListViewModel = new AlertsListViewModel();
            alertsListViewModel.Alerts = listViewModel;
            alertsListViewModel.Offset = offset;
            alertsListViewModel.Limit = realLimit;
            alertsListViewModel.Page = realPage;
            alertsListViewModel.Search = string.Empty;
            alertsListViewModel.Action = action;
            alertsListViewModel.TargetId = targetId;

            return alertsListViewModel;
        }

        private IQueryable<Entities.Alert> GetUnvisualizedAlertsQuery()
        {
            return context.Alert
                            .Where(al => !al.Visualized && al.AlertSeverity <= Enums.AlertSeverity.Warning)
                            .OrderByDescending(al => al.DateTime);
        }

        private IQueryable<Entities.Alert> GetVisualizedAlertsQuery()
        {
            return context.Alert
                            .Where(al => al.Visualized && al.AlertSeverity <= Enums.AlertSeverity.Warning)
                            .OrderByDescending(al => al.DateTime);
        }

        public async Task CreateAlertForMissingAttachmentFile(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath, string fileName)
        {
            Entities.Alert alert = new();
            alert.AlertSeverity = Enums.AlertSeverity.Warning;
            alert.Message = $"Non è stato possibile recuperare il file {fileName} nella cartella {filesDirectoryFullPath}. Controllare che sia ancora disponibile";
            alert.References = mailingPlan.Name;
            var mailingPlanDate = mailingPlan.Schedulation?.Date;
            var mailingPlanTime = mailingPlan.Schedulation?.Time;
            if (mailingPlan.Schedulation != null && mailingPlanDate != null && mailingPlanTime != null)
            {
                alert.DateTime = new DateTime(
                    mailingPlanDate.Value.Year,
                    mailingPlanDate.Value.Month,
                    mailingPlanDate.Value.Day,
                    mailingPlanTime.Value.Hour,
                    mailingPlanTime.Value.Minute,
                    mailingPlanTime.Value.Second);
            }
            alert.Visualized = false;

            context.Add(alert);
            await context.SaveChangesAsync();
        }

        public async Task CheckAlertAsync(int id)
        {
            Entities.Alert alert = await GetAlertByIdAsync(id);
            if(alert == null)
            {
                throw new Exception("Non è stato possibile recuperare l'alert cercato");
            }
            alert.Visualized = true;
            await context.SaveChangesAsync();
        }

        public async Task<DashboardViewModel> CheckAlertAndReloadModel(int id)
        {
            await CheckAlertAsync(id);
            DashboardViewModel dashboardViewModel = new();
            AlertsListViewModel unvisualizedAlertList = await GetUnvisualizedAlertListViewModelAsync(1, 10);
            AlertsListViewModel visualizedAlertList = await GetVisualizedAlertListViewModelAsync(1, 10);
            dashboardViewModel.UnvisualizedAlertList = unvisualizedAlertList;
            dashboardViewModel.VisualizedAlertList = visualizedAlertList;

            return dashboardViewModel;
        }

        private async Task RemoveAlertAsync(int id)
        {
            Entities.Alert alert = await GetAlertByIdAsync(id);
            if (alert == null)
            {
                throw new Exception("Non è stato possibile recuperare l'alert cercato");
            }
            context.Remove(alert);
            await context.SaveChangesAsync();
        }

        public async Task<AlertsListViewModel> RemoveAlertAndReload(int id)
        {
            await RemoveAlertAsync(id);
            AlertsListViewModel visualizedAlertList = await GetVisualizedAlertListViewModelAsync(1, 10);

            return visualizedAlertList;
        }

        public async Task<Entities.Alert> GetAlertByIdAsync(int id)
        {
            IQueryable<Entities.Alert> query = context.Alert
                .Where(al => al.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
