using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models;
using Microsoft.AspNetCore.Mvc;


namespace ElectronicsShop.Components
{
    public class CatalogNavItemsViewComponent : ViewComponent
    {
        private ICatalogRepository repository;
        public CatalogNavItemsViewComponent(ICatalogRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {

            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Categories);
        }
    }
}
