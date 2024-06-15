using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Models.InputModels.Attachments
{
    public class AttachmentInfo
    {
        public string Name { get; set; }
        public bool IsDeletable { get; set; }
        public ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}
