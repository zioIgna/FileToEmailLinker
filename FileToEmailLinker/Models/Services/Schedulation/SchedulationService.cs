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

            IQueryable<Entities.Schedulation> queryByWeekDay = context.WeeklySchedulation
                .Where(s => (s.Monday && date.DayOfWeek == DayOfWeek.Monday ||
                    s.Tuesday && date.DayOfWeek == DayOfWeek.Tuesday ||
                    s.Wednesday && date.DayOfWeek == DayOfWeek.Wednesday ||
                    s.Thursday && date.DayOfWeek == DayOfWeek.Thursday ||
                    s.Friday && date.DayOfWeek == DayOfWeek.Friday ||
                    s.Saturday && date.DayOfWeek == DayOfWeek.Saturday ||
                    s.Sunday && date.DayOfWeek == DayOfWeek.Sunday)
                    && s.EndDate.CompareTo(date) >= 0
                    && s.StartDate.CompareTo(date) <= 0);

            IQueryable<Entities.Schedulation> queryByDayAndMonth = context.MonthlySchedulation
                .Where(s =>
                    (s.One && date.Day == 1
                    || s.Two && date.Day == 2
                    || s.Three && date.Day == 3
                    || s.Four && date.Day == 4
                    || s.Five && date.Day == 5
                    || s.Six && date.Day == 6
                    || s.Seven && date.Day == 7
                    || s.Eight && date.Day == 8
                    || s.Nine && date.Day == 9
                    || s.Ten && date.Day == 10
                    || s.Eleven && date.Day == 11
                    || s.Twelve && date.Day == 12
                    || s.Thirteen && date.Day == 13
                    || s.Fourteen && date.Day == 14
                    || s.Fifteen && date.Day == 15
                    || s.Sixteen && date.Day == 16
                    || s.Seventeen && date.Day == 17
                    || s.Eighteen && date.Day == 18
                    || s.Nineteen && date.Day == 19
                    || s.Twenty && date.Day == 20
                    || s.Twentyone && date.Day == 21
                    || s.Twentytwo && date.Day == 22
                    || s.Twentythree && date.Day == 23
                    || s.Twentyfour && date.Day == 24
                    || s.Twentyfive && date.Day == 25
                    || s.Twentysix && date.Day == 26
                    || s.Twentyseven && date.Day == 27
                    || s.Twentyeight && date.Day == 28
                    || s.Twentynine && date.Day == 29
                    || s.Thirty && date.Day == 30
                    || s.Thirtyone && date.Day == 31)
                    &&
                    (s.January && date.Month == 1
                    || s.February && date.Month == 2
                    || s.March && date.Month == 3
                    || s.April && date.Month == 4
                    || s.May && date.Month == 5
                    || s.June && date.Month == 6
                    || s.July && date.Month == 7
                    || s.August && date.Month == 8
                    || s.September && date.Month == 9
                    || s.October && date.Month == 10
                    || s.November && date.Month == 11
                    || s.December && date.Month == 12)
                    && s.EndDate.CompareTo(date) >= 0
                    && s.StartDate.CompareTo(date) <= 0);

            ICollection<Entities.Schedulation> ongoingSchedulations = new List<Entities.Schedulation>();
            //ongoingSchedulations.AddRange(await queryByDate.ToListAsync());
            ongoingSchedulations.AddRange(await queryByWeekDay.ToListAsync());
            ongoingSchedulations.AddRange(await queryByDayAndMonth.ToListAsync());

            return ongoingSchedulations;
        }
    }
}
