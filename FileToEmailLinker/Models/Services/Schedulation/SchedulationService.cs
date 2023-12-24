using FileToEmailLinker.Data;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.Schedulation
{
    public class SchedulationService : ISchedulationService
    {
        private readonly FileToEmailLinkerContext context;

        public SchedulationService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Entities.Schedulation>> GetSchedulationsByDate(DateOnly date)
        {
            //var schedYear = date.Year;
            //var schedMonth = date.Month;
            //var schedDay = date.Day;

            IQueryable<Entities.Schedulation> query = context.Schedulation
                .Where(s => s.Date.Equals(date));

            return await query.ToListAsync();
        }
    }
}
