using FileToEmailLinker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FileToEmailLinker.Customizations
{
    public class PaginationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPaginationInfo model)
        {
            return View(model);
        }
    }
}
