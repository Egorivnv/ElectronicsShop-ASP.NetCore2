using ElectronicsShop.Models;
using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ElectronicsShop.Components
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private ICatalogRepository catalogRepository;
        public BreadcrumbViewComponent (ICatalogRepository catalogRepository)
        {
            this.catalogRepository = catalogRepository;
        }
        public IViewComponentResult Invoke(int? brand)
        {
            if (brand == null) return View( new BreadcrumbViewModel());
            string category = catalogRepository.Categories.FirstOrDefault(c => c.Brands.FirstOrDefault(b => b.BrandID == brand) != null).Name;
            return View(new BreadcrumbViewModel { Category= category,
                Brand = catalogRepository.Categories
                .FirstOrDefault(c => c.Name == category)
                .Brands.FirstOrDefault( b => b.BrandID == brand).Name 
                }
            );
        }
        //public IViewComponentResult Invoke(string category)
        //{
        //    if (category == null) return View(new BreadcrumbViewModel());
        //    return View(new BreadcrumbViewModel
        //    {
        //        Category = category,
        //    }
        //    );
        //}
    }

}
