using FileToEmailLinker.Data;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.Receiver
{
    public class ReceiverService : IReceiverService
    {
        private readonly FileToEmailLinkerContext context;

        public ReceiverService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Entities.Receiver>> GetReceiverListAsync()
        {
            IQueryable<Entities.Receiver> query = context.Receiver;

            return await query.ToListAsync();
        }

        public async Task<Entities.Receiver> GetReceiverByIdAsync(int id)
        {
            Entities.Receiver? receiver = await context.Receiver
                .Include(rec => rec.MailingPlanList)
                .FirstOrDefaultAsync(rec => rec.Id == id);

            return receiver;
        }

        public async Task<Entities.Receiver> CreateReceiverAsync(Entities.Receiver receiver)
        {
            context.Add(receiver);
            await context.SaveChangesAsync();

            return receiver;
        }

    }
}
