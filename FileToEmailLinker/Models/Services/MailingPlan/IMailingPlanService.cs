using FileToEmailLinker.Models.InputModels.MailPlans;
using FileToEmailLinker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public interface IMailingPlanService
    {
        Task<Entities.MailingPlan> GetMailingPlanByIdAsync(int id);
        Task<Entities.MailingPlan> GetMailingPlanBySchedulationIdAsync(int schedulationId);
        Task<ListViewModel<Entities.MailingPlan>> GetMailingPlanListAsync(int page, int limit, string search);
        Task<MailPlanInputModel> CreateMailPlanInputModelAsync();
        Task<Entities.MailingPlan> CreateMailingPlanAsync(MailPlanInputModel model);
        Task<MailPlanInputModel> RestoreModelForCreationAndEditing(MailPlanInputModel model);
        Task<MailPlanInputModel> GetMailingPlanEditModelAsync(int id);
        Task<Entities.MailingPlan> EditMailingPlanAsync(MailPlanInputModel model);
        Task DeleteMailingPlanAsync(Entities.MailingPlan mailingPlan);
        string GetFilesDirectoryFullPath();
        List<SelectListItem> GetPageLimitOptions();
        Task<MailingPlanListViewModel> GetMailingPlanListViewModelAsync(int page, int limit, string search);
    }
}