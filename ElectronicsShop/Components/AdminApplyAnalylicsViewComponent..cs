using ElectronicsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class AdminApplyAnalylicsViewComponent : ViewComponent
    {
        private ICatalogRepository repository;
        public AdminApplyAnalylicsViewComponent(ICatalogRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {

            return View(repository.Categories);
        }
    }
}
