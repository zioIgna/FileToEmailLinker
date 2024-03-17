using FileToEmailLinker.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileToEmailLinker.Customizations
{
    public class ReceiverInlineRowViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(Receiver model)
        {
            return View(model);
        }
    }
}
