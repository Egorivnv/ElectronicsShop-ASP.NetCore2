using ElectronicsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Components
{
    public class AdminApplyCatalogViewComponent : ViewComponent
    {
        private ICatalogRepository repository;
        public AdminApplyCatalogViewComponent(ICatalogRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            
            return View(repository.Categories);
        }
    }
}
