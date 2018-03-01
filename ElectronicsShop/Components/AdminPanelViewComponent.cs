using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class AdminPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentPage)
        {
            ViewBag.Current = currentPage;
            if (User.Identity.IsAuthenticated == true) { return View(); }
            else return View("Empty");
        }
    }
}