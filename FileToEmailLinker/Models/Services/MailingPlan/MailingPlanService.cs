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

        public async Task<Entities.MailingPlan> CreateMailingPlanAsync(MailPlanCreateInputModel model)
        {
            Entities.MailingPlan mailingPlan = new();
            mailingPlan.Name = model.Name;
            mailingPlan.ActiveState = Enums.ActiveState.Active;
            mailingPlan.Text = model.Text;
            mailingPlan.Subject = model.Subject;
            if(model.FilesSelection != null && model.FilesSelection.Count > 0) 
            {
                foreach (var file in model.FilesSelection)
                {
                    if(string.IsNullOrWhiteSpace(mailingPlan.FileStringList))
                    {
                        mailingPlan.FileStringList = file;
                    }
                    else
                    {
                        mailingPlan.FileStringList += ";" + file;
                    }
                    
                }
            }
            if(model.ReceiversSelection != null && model.ReceiversSelection.Count > 0)
            {
                foreach (var receiver in model.ReceiversSelection)
                {
                    int receiverId = Int32.Parse(receiver);
                    var receiverEntity = await receiverService.GetReceiverByIdAsync(receiverId);
                    if(receiverEntity == null)
                    {
                        throw new Exception("Receiver not found");
                    }
                    mailingPlan.ReceiverList.Add(receiverEntity);
                }
            }
            var schedulation = new Entities.Schedulation();
            schedulation.Name = model.Name;
            schedulation.StartDate = DateOnly.FromDateTime( DateTime.Today);
            schedulation.EndDate = DateOnly.FromDateTime(DateTime.MaxValue);
            schedulation.Date = model.SchedDate;
            schedulation.Time = model.SchedTime;

            mailingPlan.Schedulation = schedulation;

            context.Add(mailingPlan);
            await context.SaveChangesAsync();

            return mailingPlan;
        }
    }
}
