using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class AdminApplyOrdersViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
