using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using FileToEmailLinker.Models.InputModels.MailPlans;
using FileToEmailLinker.Models.InputModels.Schedulations;
using FileToEmailLinker.Models.Services.Receiver;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Asn1.Crmf;

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
        public async Task<Entities.MailingPlan> GetMailingPlanByIdAsync(int id)
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Include(mp => mp.MonthlySchedulation)
                .Include(mp => mp.WeeklySchedulation)
                .Include(mp => mp.ReceiverList)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Entities.MailingPlan> GetMailingPlanBySchedulationIdAsync(int schedulationId)
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Include(mp => mp.ReceiverList)
                .Include(mp => mp.WeeklySchedulation)
                .Include(mp => mp.MonthlySchedulation)
                //.Include(mp => mp.FixedDatesSchedulation)
                .Where(mp => mp.WeeklySchedulation.Id == schedulationId
                    || mp.MonthlySchedulation.Id == schedulationId
                    //|| mp.FixedDatesSchedulation.Id == schedulationId
                    );

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ICollection<Entities.MailingPlan>> GetMailingPlanListAsync()
        {
            IQueryable<Entities.MailingPlan> query = context.MailingPlan
                .Include(mp => mp.WeeklySchedulation)
                .Include(mp => mp.MonthlySchedulation);

            return await query.ToListAsync();
        }

        public async Task<MailPlanInputModel> CreateMailPlanInputModelAsync()
        {
            MailPlanInputModel mailPlanCreateInputModel = new();
            SetFileSelectListForCreate(mailPlanCreateInputModel);
            await SetReceiverSelectListForCreate(mailPlanCreateInputModel);

            return mailPlanCreateInputModel;
        }

        private async Task SetReceiverSelectListForCreate(MailPlanInputModel mailPlanCreateInputModel)
        {
            List<SelectListItem> receiversSelectList = new List<SelectListItem>();
            foreach (var receiver in await receiverService.GetReceiverListAsync())
            {
                receiversSelectList.Add(new SelectListItem { Text = receiver.Name + ' ' + receiver.Surname, Value = receiver.Id.ToString() });
            }
            mailPlanCreateInputModel.ReceiverSelectList = receiversSelectList;
        }

        private void SetFileSelectListForCreate(MailPlanInputModel mailPlanCreateInputModel)
        {
            List<SelectListItem> filesSelectList = new List<SelectListItem>();
            IEnumerable<string> files = GetFolderFiles();
            foreach (var file in files)
            {
                filesSelectList.Add(new SelectListItem { Text = Path.GetFileName(file), Value = Path.GetFileName(file) });
            }
            mailPlanCreateInputModel.FileSelectList = filesSelectList;
        }

        private IEnumerable<string> GetFolderFiles()
        {
            string fullPath = GetFilesDirectoryFullPath();
            IEnumerable<string>? files = Directory.EnumerateFiles(fullPath, "*.xlsx");
            return files;
        }

        public string GetFilesDirectoryFullPath()
        {
            var rootdir = env.ContentRootPath;
            var folderPath = configuration["HolderPath"];
            if (folderPath == null)
            {
                throw new Exception("La cartella degli allegati non è raggiungibile");
            }
            var fullPath = Path.Combine(rootdir, "wwwroot", folderPath);
            return fullPath;
        }

        public async Task<Entities.MailingPlan> CreateMailingPlanAsync(MailPlanInputModel model)
        {
            Entities.MailingPlan mailingPlan = new();
            await SetInputValues(model, mailingPlan);

            context.Add(mailingPlan);
            await context.SaveChangesAsync();

            return mailingPlan;
        }

        public async Task<Entities.MailingPlan> EditMailingPlanAsync(MailPlanInputModel model)
        {
            Entities.MailingPlan mailingPlan = await GetMailingPlanByIdAsync(model.Id);
            if(mailingPlan == null)
            {
                throw new Exception("Non è possibile recuperare la programmazione richiesta");
            }
            await SetInputValues(model, mailingPlan);

            await context.SaveChangesAsync();

            return mailingPlan;
        }

        //TODO: impostare un try catch per questo metodo
        private async Task SetInputValues(MailPlanInputModel model, Entities.MailingPlan mailingPlan)
        {
            mailingPlan.Name = model.Name;
            mailingPlan.ActiveState = model.ActiveState ? Enums.ActiveState.Active : Enums.ActiveState.Suspended;
            mailingPlan.Text = model.Text;
            mailingPlan.Subject = model.Subject;
            mailingPlan.FileStringList = string.Empty;
            if (model.FilesSelection != null && model.FilesSelection.Count > 0)
            {
                foreach (var file in model.FilesSelection)
                {
                    if (string.IsNullOrWhiteSpace(mailingPlan.FileStringList))
                    {
                        mailingPlan.FileStringList = file;
                    }
                    else
                    {
                        mailingPlan.FileStringList += ";" + file;
                    }

                }
            }
            mailingPlan.ReceiverList.Clear();
            if (model.ReceiversSelection != null && model.ReceiversSelection.Count > 0)
            {
                foreach (var receiver in model.ReceiversSelection)
                {
                    int receiverId = Int32.Parse(receiver);
                    var receiverEntity = await receiverService.GetReceiverByIdAsync(receiverId);
                    if (receiverEntity == null)
                    {
                        throw new Exception("Receiver not found");
                    }
                    mailingPlan.ReceiverList.Add(receiverEntity);
                }
            }

            Entities.Schedulation schedulation;
            if (model.WeeklySchedulation != null)
            {
                schedulation = SetWeeklySchedulation(model);
                mailingPlan.WeeklySchedulation = (WeeklySchedulation?)schedulation;
            }
            else if (model.MonthlySchedulation != null)
            {
                schedulation = SetMonthlySchedulation(model);
                mailingPlan.MonthlySchedulation = (MonthlySchedulation?)schedulation;
            }
            else
            {
                throw new Exception("Non è stata selezionata una schedulazione");
            }

            schedulation.Time = model.SchedTime;
            schedulation.StartDate = model.StartDate;
            schedulation.EndDate = model.EndDate;
        }

        private static Entities.Schedulation SetMonthlySchedulation(MailPlanInputModel model)
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

        private static Entities.Schedulation SetWeeklySchedulation(MailPlanInputModel model)
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

        public async Task<MailPlanInputModel> RestoreModelForCreationAndEditing(MailPlanInputModel model)
        {
            MailPlanInputModel restoredModel = await CreateMailPlanInputModelAsync();
            if(model.Id != 0)
            {
                restoredModel.Id = model.Id;
            }
            restoredModel.Name = model.Name;
            restoredModel.ActiveState = model.ActiveState;
            restoredModel.Subject = model.Subject;
            restoredModel.Text = model.Text;
            restoredModel.SchedTime = model.SchedTime;
            restoredModel.StartDate = model.StartDate;
            restoredModel.EndDate = model.EndDate;
            if(model.FilesSelection != null)
            {
                foreach (var originalAttachment in model.FilesSelection)
                {
                    var restoredAttachment = restoredModel.FileSelectList.FirstOrDefault(file => file.Value.Equals(originalAttachment));
                    if (restoredAttachment == null)
                    {
                        throw new Exception("Attachment not found");
                    }
                    restoredAttachment.Selected = true;
                }
            }
            if(model.ReceiversSelection != null)
            {
                foreach (var originalRecipient in model.ReceiversSelection)
                {
                    var restoredRecipient = restoredModel.ReceiverSelectList.FirstOrDefault(recepient => recepient.Value.Equals(originalRecipient));
                    if (restoredRecipient == null)
                    {
                        throw new Exception("Recipient not found");
                    }
                    restoredRecipient.Selected = true;
                }
            }
            if(model.WeeklySchedulation != null)
            {
                restoredModel.WeeklySchedulation = model.WeeklySchedulation;
            }
            else if(model.MonthlySchedulation != null)
            {
                restoredModel.MonthlySchedulation = model.MonthlySchedulation;
            }

            return restoredModel;
        }

        public async Task<MailPlanInputModel?> GetMailingPlanEditModelAsync(int id)
        {
            Entities.MailingPlan mailingPlan = await GetMailingPlanByIdAsync(id);
            if (mailingPlan is null)
            {
                return null;
            }
            MailPlanInputModel mailPlanCreateInputModel = new MailPlanInputModel();
            mailPlanCreateInputModel.Id = id;
            mailPlanCreateInputModel.Name = mailingPlan.Name;
            mailPlanCreateInputModel.Subject = mailingPlan.Subject;
            mailPlanCreateInputModel.Text = mailingPlan.Text;
            mailPlanCreateInputModel.StartDate = mailingPlan.Schedulation.StartDate;
            mailPlanCreateInputModel.EndDate = mailingPlan.Schedulation.EndDate;
            mailPlanCreateInputModel.SchedTime = mailingPlan.Schedulation.Time;
            
            SetFileSelectListForEdit(mailingPlan, mailPlanCreateInputModel);
            await SetReceiverSelectListForEdit(mailingPlan, mailPlanCreateInputModel);

            if (mailingPlan.WeeklySchedulation != null)
            {
                mailPlanCreateInputModel.WeeklySchedulation = WeeklyScheduleInputModel.FromEntity(mailingPlan.WeeklySchedulation);
            }
            else
            {
                mailPlanCreateInputModel.MonthlySchedulation = MonthlyScheduleInputModel.FromEntity(mailingPlan.MonthlySchedulation);
            }

            return mailPlanCreateInputModel;
        }

        private void SetFileSelectListForEdit(Entities.MailingPlan mailingPlan, MailPlanInputModel mailPlanCreateInputModel)
        {
            List<SelectListItem> filesSelectList = new List<SelectListItem>();
            IEnumerable<string> files = GetFolderFiles();
            foreach (var file in files)
            {
                var item = new SelectListItem { Text = Path.GetFileName(file), Value = Path.GetFileName(file) };
                if (mailingPlan.FileStringList.Split(';').Any(f => f.Equals(item.Text)))
                {
                    item.Selected = true;
                }
                filesSelectList.Add(item);
            }
            mailPlanCreateInputModel.FileSelectList = filesSelectList;
        }

        private async Task SetReceiverSelectListForEdit(Entities.MailingPlan mailingPlan, MailPlanInputModel mailPlanCreateInputModel)
        {
            List<SelectListItem> receiversSelectList = new List<SelectListItem>();
            foreach (var receiver in await receiverService.GetReceiverListAsync())
            {
                var item = new SelectListItem { Text = receiver.Name + ' ' + receiver.Surname, Value = receiver.Id.ToString() };
                if(mailingPlan.ReceiverList.Any(r => r.Id == receiver.Id))
                {
                    item.Selected = true;
                }
                receiversSelectList.Add(item);
            }
            mailPlanCreateInputModel.ReceiverSelectList = receiversSelectList;
        }

        public async Task DeleteMailingPlanAsync(Entities.MailingPlan mailingPlan)
        {
            context.Remove(mailingPlan);
            await context.SaveChangesAsync();
        }
    }
}
