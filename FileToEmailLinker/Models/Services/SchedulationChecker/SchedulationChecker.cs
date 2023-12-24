﻿using FileToEmailLinker.Models.Services.Schedulation;
using System.Timers;

namespace FileToEmailLinker.Models.Services.SchedulationChecker
{
    public class SchedulationChecker : ISchedulationChecker
    {
        private readonly ISchedulationService schedulationService;

        public SchedulationChecker(ISchedulationService schedulationService)
        {
            this.schedulationService = schedulationService;
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
                    };
                    //timers.Add(timer);
                }

            }
        }
    }
}
