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
    }
}
