using FileToEmailLinker.Models.InputModels.MailPlans;

namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public interface IMailingPlanService
    {
        Task<Entities.MailingPlan> GetMailingPlanByIdAsync(int id);
        Task<Entities.MailingPlan> GetMailingPlanBySchedulationIdAsync(int schedulationId);
        Task<ICollection<Entities.MailingPlan>> GetMailingPlanListAsync();
        Task<MailPlanInputModel> CreateMailPlanInputModelAsync();
        Task<Entities.MailingPlan> CreateMailingPlanAsync(MailPlanInputModel model);
        Task<MailPlanInputModel> RestoreModelForCreation(MailPlanInputModel model);
        Task<MailPlanInputModel> GetMailingPlanEditModelAsync(int id);
        Task<Entities.MailingPlan> EditMailingPlanAsync(MailPlanInputModel model);
        Task DeleteMailingPlanAsync(Entities.MailingPlan mailingPlan);
        string GetFilesDirectoryFullPath();
    }
}