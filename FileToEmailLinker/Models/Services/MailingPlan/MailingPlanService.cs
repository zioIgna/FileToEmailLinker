using FileToEmailLinker.Data;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public class MailingPlanService : IMailingPlanService
    {
        private readonly FileToEmailLinkerContext context;

        public MailingPlanService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }
        public async Task<Models.Entities.MailingPlan> GetMailingPlanById(int id)
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Entities.MailingPlan> GetMailingPlanBySchedulationId(int schedulationId)
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Include(mp => mp.ReceiverList)
                .Where(mp => mp.SchedulationId == schedulationId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
