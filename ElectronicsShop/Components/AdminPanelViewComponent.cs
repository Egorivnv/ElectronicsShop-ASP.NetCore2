using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class AdminPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated == true) { return View(); }
            else return View("Empty");
        }
    }
}