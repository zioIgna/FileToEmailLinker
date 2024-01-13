namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public interface IMailingPlanService
    {
        Task<Entities.MailingPlan> GetMailingPlanById(int id);
        Task<Entities.MailingPlan> GetMailingPlanBySchedulationId(int schedulationId);
    }
}