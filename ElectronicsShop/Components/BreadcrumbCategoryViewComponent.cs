using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class BreadcrumbCategoryViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(string category)
        {
            if (category == null) return View(new BreadcrumbViewModel());
            return View(new BreadcrumbViewModel
            {
                Category = category,
            }
            );
        }
    }
}
