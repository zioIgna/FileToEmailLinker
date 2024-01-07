using Azure.Core;
using FileToEmailLinker.Models.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks.Dataflow;

namespace FileToEmailLinker.Models.Services.Worker
{
    public class MailSenderHostedService : BackgroundService, IMailSenderHostedService
    {
        public MailSenderHostedService(IConfiguration configuration, IOptionsMonitor<SmtpOptions> optionsMonitor)
        {
            this.configuration = configuration;
            this.optionsMonitor = optionsMonitor;
        }
        private readonly BufferBlock<int> queue = new();
        private readonly IConfiguration configuration;
        private readonly IOptionsMonitor<SmtpOptions> optionsMonitor;

        public void EnqueueMailingPlan(int mailingPlanId)
        {
            queue.Post(mailingPlanId);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(optionsMonitor.CurrentValue.Sender));//(configuration.GetValue<string>("Smtp:Sender")
                message.To.Add(MailboxAddress.Parse("test@email.com")); 
                message.Subject = "Soggeto email";
                message.Body = new TextPart("html")
                {
                    Text = "<p>Prova di invio mail.</p>"
                };
                try
                {
                    int mailingPlanId = await queue.ReceiveAsync(stoppingToken);

                    var options = this.optionsMonitor.CurrentValue;
                    using var client = new SmtpClient();
                    await client.ConnectAsync(options.Host, options.Port);
                    if (!string.IsNullOrEmpty(options.Username))
                    {
                        await client.AuthenticateAsync(options.Username, options.Password);
                    }
                    //await this.mailMessages.SendAsync(message);
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
