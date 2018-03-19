using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Components
{
    public class FilterCatalogViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string category)
        {
            switch (category)
            {
                case "Smartphones": return View("Smartphones");
                case "Tablets": return View("Tablets");
                default: return View("Smartphones");
            }
        }
    }
}
