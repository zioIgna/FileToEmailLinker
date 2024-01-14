using FileToEmailLinker.Data;
using FileToEmailLinker.Models.InputModels.MailPlans;
using FileToEmailLinker.Models.Services.Receiver;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.MailingPlan
{
    public class MailingPlanService : IMailingPlanService
    {
        private readonly FileToEmailLinkerContext context;
        private readonly IConfiguration configuration;
        private readonly IReceiverService receiverService;
        private readonly IWebHostEnvironment env;

        public MailingPlanService(FileToEmailLinkerContext context, IConfiguration configuration, IReceiverService receiverService, IWebHostEnvironment env)
        {
            this.context = context;
            this.configuration = configuration;
            this.receiverService = receiverService;
            this.env = env;
        }
        public async Task<Entities.MailingPlan> GetMailingPlanById(int id)
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

        public async Task<ICollection<Entities.MailingPlan>> GetMailingPlanListAsync()
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Include(mp => mp.Schedulation)
                .Where(mp => mp.ActiveState == Enums.ActiveState.Active);

            return await query.ToListAsync();
        }

        public async Task<MailPlanCreateInputModel> CreateMailPlanInputModelAsync()
        {
            MailPlanCreateInputModel mailPlanCreateInputModel = new();
            List<SelectListItem> filesSelectList = new List<SelectListItem>();
            var rootdir = env.ContentRootPath;
            var folderPath = configuration["HolderPath"];
            if(folderPath == null)
            {
                throw new Exception("La cartella degli allegati non è raggiungibile");
            }
            var fullPath = Path.Combine(rootdir,"wwwroot", folderPath);
            //DirectoryInfo dir = new DirectoryInfo(fullPath);
            //FileInfo[] files = dir.GetFiles("*.xlsx");
            IEnumerable<string>? files = Directory.EnumerateFiles(fullPath, "*.xlsx");
            foreach (var file in files)
            {
                filesSelectList.Add(new SelectListItem { Text = Path.GetFileName(file), Value = Path.GetFileName(file) });
            }
            mailPlanCreateInputModel.FileSelectList = filesSelectList;
            List<SelectListItem> receiversSelectList = new List<SelectListItem>();
            foreach (var receiver in await receiverService.GetReceiverListAsync())
            {
                receiversSelectList.Add(new SelectListItem { Text = receiver.Name + ' ' + receiver.Surname, Value = receiver.Id.ToString() });
            }
            mailPlanCreateInputModel.ReceiverSelectList = receiversSelectList;

            return mailPlanCreateInputModel;
        }
    }
}
