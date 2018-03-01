using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElectronicsShop.Models;
using ElectronicsShop.Models.ViewModels;

namespace ElectronicsShop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 6;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
            Products = repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = pageSize,
                TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(e => e.Category == category).Count()
            },
            CurrentCategory = category
            });

        [HttpGet]
        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult Item(int ProductID)
        {
            Product product = repository.Products
                .Where(p => p.ProductID == ProductID)
                .FirstOrDefault();
            return View(product);
        }
    }
}