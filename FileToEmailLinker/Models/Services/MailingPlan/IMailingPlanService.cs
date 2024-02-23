using FileToEmailLinker.Models.InputModels.MailPlans;

namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public interface IMailingPlanService
    {
        Task<Entities.MailingPlan> GetMailingPlanById(int id);
        Task<Entities.MailingPlan> GetMailingPlanBySchedulationId(int schedulationId);
        Task<ICollection<Entities.MailingPlan>> GetMailingPlanListAsync();
        Task<MailPlanCreateInputModel> CreateMailPlanInputModelAsync();
        Task<Entities.MailingPlan> CreateMailingPlanAsync(MailPlanCreateInputModel model);
        Task<MailPlanCreateInputModel> RestoreModelForCreation(MailPlanCreateInputModel model);
        Task<MailPlanCreateInputModel> GetMailingPlanEditModelAsync(int id);
    }
}