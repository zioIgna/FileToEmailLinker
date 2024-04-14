namespace FileToEmailLinker.Models.Services.Alert
{
    public interface IAlertService
    {
        Task CreateAlertForMissingAttachmentFile(Entities.MailingPlan mailingPlan, string filesDirectoryFullPath, string fileName);
        Task<ICollection<Entities.Alert>> GetUnvisualizedAlertListAsync();
        Task<Entities.Alert> GetAlertByIdAsync(int id);
        Task CheckAlertAsync(int id);
        Task<ICollection<Entities.Alert>?> GetVisualizedAlertListAsync();
    }
}