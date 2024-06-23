using FileToEmailLinker.Models.InputModels.Attachments;

namespace FileToEmailLinker.Models.ViewModels
{
    public class AttachmentListViewModel
    {
        public IFormFile Attachment { get; set; }
        public ICollection<AttachmentInfo> Allegati { get; set; }
        public string TestField { get; set; }
    }
}
