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
        private ICatalogRepository catalogRepository;
        public int pageSize = 6;
        public ProductController(IProductRepository repo, ICatalogRepository catalogRepository)
        {
            repository = repo;
            this.catalogRepository = catalogRepository;
        }

        public ViewResult List(string category, int productPage = 1)
        {
            var productsList = new ProductsListViewModel
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
            };
            if (productsList.Products.Count() == 0) return View("ShowBrandItems");
            return View(productsList);
        }

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
        [HttpGet]
        public ViewResult ShowBrandItems(string category, int brand, int productPage = 1)
        {
            ProductsListViewModel productsList = new ProductsListViewModel
            {
                Products = repository.Products
               .Where(p => p.Category == category && p.Brand == brand)
               .OrderBy(p => p.ProductID)
               .Skip((productPage - 1) * pageSize)
               .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(e => e.Category == category && e.Brand == brand).Count()
                },
                CurrentCategory = category,
                CurrentBrand = brand
            };
            if (productsList.Products.Count() == 0) return View();
            return View("ShowBrandItemsList", productsList);
        }
    }
}