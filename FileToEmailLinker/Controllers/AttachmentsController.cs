using FileToEmailLinker.Models.InputModels.Attachments;
using FileToEmailLinker.Models.Services.Attachment;
using FileToEmailLinker.Models.ViewModels;
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
            //AttachmentListViewModel model = new();
            //model.Allegati = allegati;
            return View(allegati);
        }

        [HttpPost]
        public async Task<IActionResult> AddAttachment(IFormFile attachment)
        {
            if(attachment != null)
            {
                if (await attachmentService.FileAlreadyExists(attachment))
                {
                    TempData["ErrorMessage"] = "Esiste già un file con lo stesso nome. Eliminare il file esistente o rinominare il file che si intende caricare";
                    return RedirectToAction(nameof(Index));
                }
                try
                {
                    attachmentService.UploadFile(attachment);
                }
                catch (Exception ex)
                {

                    TempData["ErrorMessage"] = $"Errore di caricamento: {ex.Message}. Non è stato possibile caricare il file. Riprovare più tardi";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ConfirmationMessage"] = "File caricato con successo";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Selezionare un file per il caricamento";
            return RedirectToAction(nameof(Index));
        }
    }
}
