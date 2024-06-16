using FileToEmailLinker.Models.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FileToEmailLinker.Models.InputModels.Attachments
{
    public class AttachmentInfo
    {
        public string Name { get; set; }
        public bool IsDeletable { get => MailingPlanList == null || MailingPlanList.Count() == 0; }
        public ICollection<MailingPlan> MailingPlanList { get; set; } = new List<MailingPlan>();
    }
}
