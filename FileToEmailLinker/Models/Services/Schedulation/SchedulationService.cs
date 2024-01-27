using FileToEmailLinker.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace FileToEmailLinker.Models.Services.Schedulation
{
    public class SchedulationService : ISchedulationService
    {
        private readonly FileToEmailLinkerContext context;

        public SchedulationService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Entities.Schedulation>> GetSchedulationsByDateOrWeekDay(DateOnly date)
        {
            //IQueryable<Entities.Schedulation> queryByDate = context.Schedulation
            //    .Where(s => s.Date.Equals(date));

            //IQueryable<Entities.Schedulation> queryByWeekDay = context.Schedulation
            //    .Where(s => (s.Monday && date.DayOfWeek == DayOfWeek.Monday ||
            //        s.Tuesday && date.DayOfWeek == DayOfWeek.Tuesday ||
            //        s.Wednesday && date.DayOfWeek == DayOfWeek.Wednesday ||
            //        s.Thursday && date.DayOfWeek == DayOfWeek.Thursday ||
            //        s.Friday && date.DayOfWeek == DayOfWeek.Friday ||
            //        s.Saturday && date.DayOfWeek == DayOfWeek.Saturday ||
            //        s.Sunday && date.DayOfWeek == DayOfWeek.Sunday)
            //        && s.EndDate.CompareTo(date)>= 0
            //        && s.StartDate.CompareTo(date)<= 0);

            //ICollection<Entities.Schedulation> ongoingSchedulations = new List<Entities.Schedulation>();
            //ongoingSchedulations.AddRange(await queryByDate.ToListAsync());
            //ongoingSchedulations.AddRange(await queryByWeekDay.ToListAsync());

            //return ongoingSchedulations;
            throw new NotImplementedException();
        }
    }
}
