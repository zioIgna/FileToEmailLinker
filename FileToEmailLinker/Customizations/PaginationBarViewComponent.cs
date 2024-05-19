using FileToEmailLinker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FileToEmailLinker.Customizations
{
    public class PaginationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPaginationInfo model)
        {
            if (!string.IsNullOrEmpty(model.Action))
            {
                return View("ForReplacement", model);
            }
            return View("Default",model);
        }
    }
}
