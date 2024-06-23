using Azure.Core;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.Exceptions;
using FileToEmailLinker.Models.Options;
using FileToEmailLinker.Models.Services.Alert;
using FileToEmailLinker.Models.Services.Attachment;
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
                    int mailingPlanId = await queue.ReceiveAsync(stoppingToken);

                    Console.WriteLine($"Ottenuta la schedulazione {mailingPlanId}");

                    using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                    IMailingPlanService mailingPlanService = serviceProvider.GetRequiredService<IMailingPlanService>();
                    IAttachmentService attachmentService = serviceProvider.GetRequiredService<IAttachmentService>();

                    Entities.MailingPlan mailingPlan = await mailingPlanService.GetMailingPlanByIdAsync(mailingPlanId);
                    
                    if (mailingPlan == null)
                    {
                        Console.WriteLine($"Non si è riconosciuto il MailingPlan");
                        throw new MailingPlanNotFoundException(mailingPlanId);
                    }
                    Console.WriteLine($"Riconosciuto il MailingPlan {mailingPlan.Name}");
                    string filesDirectoryFullPath = attachmentService.GetFilesDirectoryFullPath();

                    MimeMessage message = await CreateMimeMessage(mailingPlan, filesDirectoryFullPath);
                    using SmtpClient client = await SendEmail(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è ottenuta la eccezione {ex.Message}");
                }
            }
        }

        private async Task<SmtpClient> SendEmail(MimeMessage message)
        {
            var options = this.optionsMonitor.CurrentValue;
            var client = new SmtpClient();
            await client.ConnectAsync(options.Host, options.Port);
            if (!string.IsNullOrEmpty(options.Username))
            {
                await client.AuthenticateAsync(options.Username, options.Password);
            }
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return client;
        }

        private async Task<MimeMessage> CreateMimeMessage(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(optionsMonitor.CurrentValue.Sender));
            InternetAddressList internetAddresses = new InternetAddressList();
            internetAddresses.AddRange(mailingPlan.ReceiverList
                .Select(receiver => new MailboxAddress(string.Concat(receiver.Name, ' ', receiver.Surname), receiver.Email)));
            message.To.AddRange(internetAddresses);
            message.Subject = mailingPlan.Subject;

            var body = new TextPart("html")
            {
                Text = $"<p>{mailingPlan.Text}</p>"
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            await AddAttachments(mailingPlan, filesDirectoryFullPath, body, multipart);

            message.Body = multipart;
            return message;
        }

        private async Task AddAttachments(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath, TextPart body, Multipart multipart)
        {
            string[] fileNames = mailingPlan.FileStringList.Split(";");
            foreach (var fileName in fileNames)
            {
                string fileNameWithPath = Path.Combine(filesDirectoryFullPath, fileName);
                if (!File.Exists(fileNameWithPath))
                {
                    using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                    IAlertService alertService = serviceProvider.GetService<IAlertService>();
                    await alertService.CreateAlertForMissingAttachmentFile(mailingPlan, filesDirectoryFullPath, fileName);
                    body.Text += $"<hr /><p>Non è stato possibile recuperare il file {fileName}</p>";

                    continue;
                }
                var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Content = new MimeContent(File.OpenRead(fileNameWithPath), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Binary,
                    FileName = Path.GetFileName(fileNameWithPath)
                };
                multipart.Add(attachment);
            }
        }
    }
}
