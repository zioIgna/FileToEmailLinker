using Microsoft.AspNetCore.Mvc;

namespace FileToEmailLinker.Controllers
{
    public class AllegatiController : Controller
    {
        public IActionResult Index()
        {
            string[] allegati = { "file1", "file2", "file3", "file4" };
            return View(allegati);
        }
    }
}
