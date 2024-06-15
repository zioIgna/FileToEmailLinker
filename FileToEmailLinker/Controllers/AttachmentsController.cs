using FileToEmailLinker.Models.InputModels.Attachments;
using FileToEmailLinker.Models.Services.Attachment;
using Microsoft.AspNetCore.Mvc;

namespace FileToEmailLinker.Controllers
{
    public class AttachmentsController : Controller
    {
        private readonly IAttachmentService attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            this.attachmentService = attachmentService;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<AttachmentInfo> allegati = await attachmentService.GetAttachments(); //{ "file1", "file2", "file3", "file4" };
            return View(allegati);
        }
    }
}
