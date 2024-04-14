using FileToEmailLinker.Data;
using FileToEmailLinker.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileToEmailLinker.Models.Services.Alert
{
    public class AlertService : IAlertService
    {
        private readonly FileToEmailLinkerContext context;

        public AlertService(FileToEmailLinkerContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Entities.Alert>> GetUnvisualizedAlertListAsync()
        {
            IQueryable<Entities.Alert> query = context.Alert
                .Where(al => !al.Visualized && al.AlertSeverity <= Enums.AlertSeverity.Warning);

            return await query.ToListAsync();
        }

        public async Task CreateAlertForMissingAttachmentFile(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath, string fileName)
        {
            Entities.Alert alert = new();
            alert.AlertSeverity = Enums.AlertSeverity.Warning;
            alert.Message = $"Non è stato possibile recuperare il file {fileName} nella cartella {filesDirectoryFullPath}. Controllare che sia ancora disponibile";
            alert.References = mailingPlan.Name;
            var mailingPlanDate = mailingPlan.Schedulation?.Date;
            var mailingPlanTime = mailingPlan.Schedulation?.Time;
            if (mailingPlan.Schedulation != null && mailingPlanDate != null && mailingPlanTime != null)
            {
                alert.DateTime = new DateTime(
                    mailingPlanDate.Value.Year,
                    mailingPlanDate.Value.Month,
                    mailingPlanDate.Value.Day,
                    mailingPlanTime.Value.Hour,
                    mailingPlanTime.Value.Minute,
                    mailingPlanTime.Value.Second);
            }
            alert.Visualized = false;

            context.Add(alert);
            await context.SaveChangesAsync();
        }

        public async Task CheckAlertAsync(int id)
        {
            Entities.Alert alert = await GetAlertByIdAsync(id);
            if(alert == null)
            {
                throw new Exception("Non è stato possibile recuperare l'alert cercato");
            }
            alert.Visualized = true;
            await context.SaveChangesAsync();
        }

        public async Task<Entities.Alert> GetAlertByIdAsync(int id)
        {
            IQueryable<Entities.Alert> query = context.Alert
                .Where(al => al.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
