namespace FileToEmailLinker.Models.Services.Worker
{
    public interface IMailSenderHostedService
    {
        void EnqueueMailingPlan(int mailingPlanId);
    }
}