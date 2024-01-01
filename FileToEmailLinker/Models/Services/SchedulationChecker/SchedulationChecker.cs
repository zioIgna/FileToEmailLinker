using FileToEmailLinker.Models.Services.Schedulation;
using FileToEmailLinker.Models.Services.Worker;
using System.Timers;

namespace FileToEmailLinker.Models.Services.SchedulationChecker
{
    public class SchedulationChecker : ISchedulationChecker
    {
        private readonly ISchedulationService schedulationService;
        private readonly IServiceScopeFactory serviceScopeFactory;

        //private readonly IMailSenderHostedService mailSenderHostedService;

        public SchedulationChecker(
            ISchedulationService schedulationService
            //, IMailSenderHostedService mailSenderHostedService
            , IServiceScopeFactory serviceScopeFactory
            )
        {
            this.schedulationService = schedulationService;
            this.serviceScopeFactory = serviceScopeFactory;
            //this.mailSenderHostedService = mailSenderHostedService;
        }

        private async Task<ICollection<Entities.Schedulation>> GetNextDaySchedulations()
        {
            var tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1).Date);
            var today = DateOnly.FromDateTime(DateTime.Now.Date);
            ICollection<Entities.Schedulation> schedulations = await schedulationService.GetSchedulationsByDate(today);

            return schedulations;
        }

        public async Task SetSchedulationsTimers()
        {
            //var timers = new List<System.Timers.Timer>();
            Console.WriteLine("Avviato il metodo SetSchedulationsTimers");
            var schedulations = await GetNextDaySchedulations();
            int ordinale = 0;
            foreach (var schedulation in schedulations)
            {
                var missingSeconds = (schedulation.Date.ToDateTime(schedulation.Time) - DateTime.Now).TotalSeconds;
                Console.WriteLine($"I secondi mancanti alla {++ordinale}^ esecuzione sono {missingSeconds}");
                if (missingSeconds >= 0)
                {
                    Console.WriteLine($"Impostato il timer per la schedulazione {schedulation.Name}");
                    System.Timers.Timer timer = new System.Timers.Timer(missingSeconds * 1000);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Elapsed += (sender, e) =>
                    {
                        Console.WriteLine($"La schedulazione riconosciuta è la {schedulation.Name}");
                        try
                        {
                            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                            IMailSenderHostedService mailSenderHostedService = serviceProvider.GetRequiredService<IMailSenderHostedService>();

                            mailSenderHostedService.EnqueueMailingPlan(schedulation.Id);
                            timer.Stop();
                            timer.Dispose();
                        }
                        catch (Exception exc)
                        {

                            throw new Exception(exc.Message);
                        }
                    };
                    //timers.Add(timer);
                }

            }
        }
    }
}
