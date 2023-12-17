using System.Drawing.Text;

namespace FileToEmailLinker.Models.Services.Worker
{
    public class DailySchedulesReader : BackgroundService
    {
        private PeriodicTimer _startingTimer = new (TimeSpan.FromSeconds(1));
        //private PeriodicTimer _executionTimer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(await _startingTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Lanciata l'applicazione alle ore: " + DateTime.Now.ToString("O"));
                DateTime adesso = DateTime.Now;

                if (adesso.Hour == 22 && adesso.Minute == 33)
                {
                    Console.Write("Riconosciuta l'ora alle: " + DateTime.Now.ToString("O"));
                    _startingTimer = new(TimeSpan.FromSeconds(2));
                    while (await _startingTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Resettato il timer alle ore: " + DateTime.Now.ToString("G"));
                    }
                }
            }
        }
    }
}
