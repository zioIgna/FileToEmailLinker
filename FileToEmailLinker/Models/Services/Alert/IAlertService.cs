using FileToEmailLinker.Models.ViewModels;
using FileToEmailLinker.Models.ViewModels.Dashboard;

namespace FileToEmailLinker.Models.Services.Alert
{
    public interface IAlertService
    {
        Task CreateAlertForMissingAttachmentFile(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath, string fileName);
        Task<ICollection<Entities.Alert>> GetUnvisualizedAlertListAsync();
        Task<Entities.Alert> GetAlertByIdAsync(int id);
        Task CheckAlertAsync(int id);
        Task<ICollection<Entities.Alert>?> GetVisualizedAlertListAsync();
        Task<AlertsListViewModel> GetUnvisualizedAlertListViewModelAsync(int page, int limit);
        Task<AlertsListViewModel> GetVisualizedAlertListViewModelAsync(int page, int limit);
    }
}