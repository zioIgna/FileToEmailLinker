using FileToEmailLinker.Models.Services.SchedulationChecker;
using System.Drawing.Text;

namespace FileToEmailLinker.Models.Services.Worker
{
    public class DailySchedulesReaderService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private PeriodicTimer _startingTimer = new (TimeSpan.FromSeconds(1));
        private bool timerSetTo24Hour = false;


        public DailySchedulesReaderService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        //private PeriodicTimer _executionTimer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(await _startingTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Lanciata l'applicazione alle ore: " + DateTime.Now.ToString("O"));
                DateTime adesso = DateTime.Now;

                if (!timerSetTo24Hour && adesso.Hour == 16 && adesso.Minute == 27)
                {
                    Console.Write("Riconosciuta l'ora alle: " + DateTime.Now.ToString("O"));
                    _startingTimer = new(TimeSpan.FromHours(24));
                    //_startingTimer = new(TimeSpan.FromMinutes(4));
                    timerSetTo24Hour = true;
                    //if(!timerSetTo24Hour)
                    //{
                    //    _startingTimer = new(TimeSpan.FromHours(24));
                    //}
                }
                if (timerSetTo24Hour)
                {
                    using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
                    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                    ISchedulationChecker schedulationChecker = serviceProvider.GetRequiredService<ISchedulationChecker>();

                    int schedulationsCount = await schedulationChecker.SetSchedulationsTimers();
                    Console.WriteLine($"Settate le schedulazioni: {schedulationsCount.ToString()}");
                    //while (await _startingTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
                    //{
                    //    Console.WriteLine("Resettato il timer alle ore: " + DateTime.Now.ToString("G"));
                    //}
                }
            }
        }
    }
}
