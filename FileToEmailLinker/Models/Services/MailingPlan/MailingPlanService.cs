using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.InputModels.MailPlans;
using FileToEmailLinker.Models.InputModels.Schedulations;
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
                .Include(mp => mp.WeeklySchedulation)
                .Include(mp => mp.MonthlySchedulation)
                .Include(mp => mp.FixedDatesSchedulation)
                .Where(mp => mp.WeeklySchedulation.Id == schedulationId
                    || mp.MonthlySchedulation.Id == schedulationId
                    || mp.FixedDatesSchedulation.Id == schedulationId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ICollection<Entities.MailingPlan>> GetMailingPlanListAsync()
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                //.Include(mp => mp.Schedulation)
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
            //WeeklyScheduleInputModel weeklySchedulation = new();
            //WeeklySchedulation weeklySchedulation = new();
            //mailPlanCreateInputModel.WeeklySchedulation = weeklySchedulation;

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

            Entities.Schedulation schedulation;
            if(model.WeeklySchedulation != null)
            {
                schedulation = SetWeeklySchedulation(model);
                mailingPlan.WeeklySchedulation = (WeeklySchedulation?)schedulation;
            }
            else if(model.MonthlySchedulation != null)
            {
                schedulation = SetMonthlySchedulation(model);
                mailingPlan.MonthlySchedulation = (MonthlySchedulation?)schedulation;
            }
            else
            {
                //schedulation = new Entities.FixedDatesSchedulation();
                //schedulation.Date = model.SchedDate;
                //mailingPlan.FixedDatesSchedulation = (FixedDatesSchedulation?)schedulation;
                throw new Exception("Non è stata selezionata una schedulazione");
            }

            //var schedulation = new Entities.Schedulation();
            //schedulation.Name = model.Name;
            //schedulation.StartDate = DateOnly.FromDateTime( DateTime.Today);
            //schedulation.EndDate = DateOnly.FromDateTime(DateTime.MaxValue);
            //schedulation.Monday = model.Monday;
            //schedulation.Tuesday = model.Tuesday;
            //schedulation.Wednesday = model.Wednesday;
            //schedulation.Thursday = model.Thursday;
            //schedulation.Friday = model.Friday;
            //schedulation.Saturday = model.Saturday;
            //schedulation.Sunday = model.Sunday;
            schedulation.Time = model.SchedTime;

            schedulation.StartDate = model.StartDate;
            schedulation.EndDate = model.EndDate;

            //mailingPlan.Schedulation = schedulation;

            context.Add(mailingPlan);
            await context.SaveChangesAsync();

            return mailingPlan;
        }

        private static Entities.Schedulation SetMonthlySchedulation(MailPlanCreateInputModel model)
        {
            Entities.Schedulation schedulation = new MonthlySchedulation();
            ((MonthlySchedulation)schedulation).One = model.MonthlySchedulation.One;
            ((MonthlySchedulation)schedulation).Two = model.MonthlySchedulation.Two;
            ((MonthlySchedulation)schedulation).Three = model.MonthlySchedulation.Three;
            ((MonthlySchedulation)schedulation).Four = model.MonthlySchedulation.Four;
            ((MonthlySchedulation)schedulation).Five = model.MonthlySchedulation.Five;
            ((MonthlySchedulation)schedulation).Six = model.MonthlySchedulation.Six;
            ((MonthlySchedulation)schedulation).Seven = model.MonthlySchedulation.Seven;
            ((MonthlySchedulation)schedulation).Eight = model.MonthlySchedulation.Eight;
            ((MonthlySchedulation)schedulation).Nine = model.MonthlySchedulation.Nine;
            ((MonthlySchedulation)schedulation).Ten = model.MonthlySchedulation.Ten;
            ((MonthlySchedulation)schedulation).Eleven = model.MonthlySchedulation.Eleven;
            ((MonthlySchedulation)schedulation).Twelve = model.MonthlySchedulation.Twelve;
            ((MonthlySchedulation)schedulation).Thirteen = model.MonthlySchedulation.Thirteen;
            ((MonthlySchedulation)schedulation).Fourteen = model.MonthlySchedulation.Fourteen;
            ((MonthlySchedulation)schedulation).Fifteen = model.MonthlySchedulation.Fifteen;
            ((MonthlySchedulation)schedulation).Sixteen = model.MonthlySchedulation.Sixteen;
            ((MonthlySchedulation)schedulation).Seventeen = model.MonthlySchedulation.Seventeen;
            ((MonthlySchedulation)schedulation).Eighteen = model.MonthlySchedulation.Eighteen;
            ((MonthlySchedulation)schedulation).Nineteen = model.MonthlySchedulation.Nineteen;
            ((MonthlySchedulation)schedulation).Twenty = model.MonthlySchedulation.Twenty;
            ((MonthlySchedulation)schedulation).Twentyone = model.MonthlySchedulation.Twentyone;
            ((MonthlySchedulation)schedulation).Twentytwo = model.MonthlySchedulation.Twentytwo;
            ((MonthlySchedulation)schedulation).Twentythree = model.MonthlySchedulation.Twentythree;
            ((MonthlySchedulation)schedulation).Twentyfour = model.MonthlySchedulation.Twentyfour;
            ((MonthlySchedulation)schedulation).Twentyfive = model.MonthlySchedulation.Twentyfive;
            ((MonthlySchedulation)schedulation).Twentysix = model.MonthlySchedulation.Twentysix;
            ((MonthlySchedulation)schedulation).Twentyseven = model.MonthlySchedulation.Twentyseven;
            ((MonthlySchedulation)schedulation).Twentyeight = model.MonthlySchedulation.Twentyeight;
            ((MonthlySchedulation)schedulation).Twentynine = model.MonthlySchedulation.Twentynine;
            ((MonthlySchedulation)schedulation).Thirty = model.MonthlySchedulation.Thirty;
            ((MonthlySchedulation)schedulation).Thirtyone = model.MonthlySchedulation.Thirtyone;
            ((MonthlySchedulation)schedulation).January = model.MonthlySchedulation.January;
            ((MonthlySchedulation)schedulation).February = model.MonthlySchedulation.February;
            ((MonthlySchedulation)schedulation).March = model.MonthlySchedulation.March;
            ((MonthlySchedulation)schedulation).April = model.MonthlySchedulation.April;
            ((MonthlySchedulation)schedulation).May = model.MonthlySchedulation.May;
            ((MonthlySchedulation)schedulation).June = model.MonthlySchedulation.June;
            ((MonthlySchedulation)schedulation).July = model.MonthlySchedulation.July;
            ((MonthlySchedulation)schedulation).August = model.MonthlySchedulation.August;
            ((MonthlySchedulation)schedulation).September = model.MonthlySchedulation.September;
            ((MonthlySchedulation)schedulation).October = model.MonthlySchedulation.October;
            ((MonthlySchedulation)schedulation).November = model.MonthlySchedulation.November;
            ((MonthlySchedulation)schedulation).December = model.MonthlySchedulation.December;
            return schedulation;
        }

        private static Entities.Schedulation SetWeeklySchedulation(MailPlanCreateInputModel model)
        {
            Entities.Schedulation schedulation = new WeeklySchedulation();
            ((WeeklySchedulation)schedulation).Monday = model.WeeklySchedulation.Monday;
            ((WeeklySchedulation)schedulation).Tuesday = model.WeeklySchedulation.Tuesday;
            ((WeeklySchedulation)schedulation).Wednesday = model.WeeklySchedulation.Wednesday;
            ((WeeklySchedulation)schedulation).Thursday = model.WeeklySchedulation.Thursday;
            ((WeeklySchedulation)schedulation).Friday = model.WeeklySchedulation.Friday;
            ((WeeklySchedulation)schedulation).Saturday = model.WeeklySchedulation.Saturday;
            ((WeeklySchedulation)schedulation).Sunday = model.WeeklySchedulation.Sunday;
            return schedulation;
        }
    }
}
