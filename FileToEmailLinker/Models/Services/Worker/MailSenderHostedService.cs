using Azure.Core;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Exceptions;
using FileToEmailLinker.Models.Options;
using FileToEmailLinker.Models.Services.MailingPlan;
using FileToEmailLinker.Models.Services.SchedulationChecker;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks.Dataflow;

namespace FileToEmailLinker.Models.Services.Worker
{
    public class MailSenderHostedService : BackgroundService, IMailSenderHostedService
    {
        public MailSenderHostedService(
            IConfiguration configuration,
            IOptionsMonitor<SmtpOptions> optionsMonitor,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            this.configuration = configuration;
            this.optionsMonitor = optionsMonitor;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        private readonly BufferBlock<int> queue = new();
        private readonly IConfiguration configuration;
        private readonly IOptionsMonitor<SmtpOptions> optionsMonitor;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public void EnqueueMailingPlan(int mailingPlanId)
        {
            queue.Post(mailingPlanId);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //MimeMessage message = new MimeMessage();
                //message.From.Add(MailboxAddress.Parse(optionsMonitor.CurrentValue.Sender));//(configuration.GetValue<string>("Smtp:Sender")
                //message.To.Add(MailboxAddress.Parse("test@email.com")); 
                //message.Subject = "Soggeto email";
                //message.Body = new TextPart("html")
                //{
                //    Text = "<p>Prova di invio mail.</p>"
                //};
                try
                {
                    int schedulationId = await queue.ReceiveAsync(stoppingToken);

                    using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                    IMailingPlanService mailingPlanService = serviceProvider.GetRequiredService<IMailingPlanService>();

                    Entities.MailingPlan mailingPlan = await mailingPlanService.GetMailingPlanBySchedulationId(schedulationId);
                    if(mailingPlan == null)
                    {
                        throw new MailingPlanNotFoundException(schedulationId);
                    }
                    MimeMessage message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(optionsMonitor.CurrentValue.Sender));
                    InternetAddressList internetAddresses = new InternetAddressList();
                    internetAddresses.AddRange(mailingPlan.ReceiverList
                        .Select(receiver => new MailboxAddress(string.Concat(receiver.Name, ' ', receiver.Surname), receiver.Email)));
                    message.To.AddRange(internetAddresses);
                    message.Subject = "Invio files excel";
                    message.Body = new TextPart("html")
                    {
                        Text = $"<p>{mailingPlan.Text}</p>"
                    };

                    var options = this.optionsMonitor.CurrentValue;
                    using var client = new SmtpClient();
                    await client.ConnectAsync(options.Host, options.Port);
                    if (!string.IsNullOrEmpty(options.Username))
                    {
                        await client.AuthenticateAsync(options.Username, options.Password);
                    }
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
