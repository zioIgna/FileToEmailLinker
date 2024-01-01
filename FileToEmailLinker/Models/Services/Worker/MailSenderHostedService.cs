using System.Threading.Tasks.Dataflow;

namespace FileToEmailLinker.Models.Services.Worker
{
    public class MailSenderHostedService : BackgroundService, IMailSenderHostedService
    {
        private readonly BufferBlock<int> queue = new();

        public void EnqueueMailingPlan(int mailingPlanId)
        {
            queue.Post(mailingPlanId);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    int mailingPlanId = await queue.ReceiveAsync(stoppingToken);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
