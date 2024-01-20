using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
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
            return await context.Receiver
                .FirstOrDefaultAsync(rec => rec.Id == id);
        }
    }
}
