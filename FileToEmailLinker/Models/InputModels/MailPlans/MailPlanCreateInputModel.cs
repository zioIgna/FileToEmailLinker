using FileToEmailLinker.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileToEmailLinker.Models.InputModels.MailPlans
{
    public class MailPlanCreateInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ActiveState ActiveState { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> FilesSelection { get; set; }
        public List<string> ReceiversSelection { get; set; }
        public List<SelectListItem> FileSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ReceiverSelectList { get; set; } = new List<SelectListItem>();
        public DateOnly SchedDate { get; set; }
        public TimeOnly SchedTime { get; set; }
    }
}
