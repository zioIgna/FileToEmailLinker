using FileToEmailLinker.Data;
using FileToEmailLinker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FileToEmailLinker.Models.Services.Receiver
{
    public class ReceiverService : IReceiverService
    {
        private readonly FileToEmailLinkerContext context;
        private readonly IConfiguration configuration;

        public ReceiverService(FileToEmailLinkerContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<ICollection<Entities.Receiver>> GetReceiverListAsync()
        {
            IQueryable<Entities.Receiver> query = context.Receiver;

            return await query.ToListAsync();
        }

        public async Task<ListViewModel<Entities.Receiver>> GetReceiverListForViewModelAsync(int page, int limit, string search)
        {
            int realPage = Math.Max(1, page);
            int realLimit = Math.Max(1, limit);
            int offset = (realPage - 1) * realLimit;

            IQueryable<Entities.Receiver> query = context.Receiver
                .Where(r => r.Name.ToUpper().Contains(search.ToUpper()) 
                || r.Surname.ToUpper().Contains(search.ToUpper()) 
                || r.Email.ToUpper().Contains(search.ToUpper()));

            List<Entities.Receiver> receivers = await query
                .Skip(offset)
                .Take(realLimit)
                .ToListAsync();

            int totalCount = await query.CountAsync();

            ListViewModel<Entities.Receiver> listViewModel = new ListViewModel<Entities.Receiver>
            {
                Results = receivers,
                TotalCount = totalCount
            };

            return listViewModel;
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

        public async Task<ReceiverListViewModel> GetReceiverListViewModelAsync(int page, int limit, string search)
        {
            ReceiverListViewModel model = new();
            ListViewModel<Entities.Receiver> receiverListView = await GetReceiverListForViewModelAsync(page, limit, search);
            List<SelectListItem> pageLimitOptions = GetPageLimitOptions();
            model.ReceiverList = receiverListView;
            model.Page = page;
            model.Limit = limit;
            model.Search = search;
            model.PageLimitOptions = pageLimitOptions;

            return model;
        }

        private List<SelectListItem> GetPageLimitOptions()
        {
            int[] values = configuration.GetSection("DropdownOptions").GetSection("Receiver").Get<int[]>();
            List<SelectListItem> options = new List<SelectListItem>();
            options.AddRange(values.Select(val => new SelectListItem(val.ToString(), val.ToString(), values.ElementAt(0).Equals(val))));

            return options;
        }
    }
}
