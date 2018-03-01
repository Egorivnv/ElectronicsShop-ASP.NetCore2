using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models;
using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Controllers
{
    [Authorize]
    public class AdminStockController : Controller
    {
        private IProductRepository repository;
        private ICatalogRepository categoryRepository;
        private IProductStockRepository stockRepository;
        public AdminStockController(IProductRepository repo, ICatalogRepository catergoryRep, IProductStockRepository stockRep)
        {
            repository = repo;
            categoryRepository = catergoryRep;
            stockRepository = stockRep;
        }

        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult ShowStockTable(string category, int brand)
        {
                IQueryable<Product> productList = null;
                if (category == "all") productList = repository.Products;
                if (category != "all" && brand == 1) productList = repository.Products.Where(p => p.Category == category);
                if (category != "all" && brand != 1) productList = repository.Products.Where(p => p.Category == category).Where(p => p.Brand == brand);

                List<ProductWithStockViewModel> productStockList = new List<ProductWithStockViewModel>();
                foreach (var product in productList)
                {
                    var stock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID);
                    if (stock == null) stock = new ProductStock();
                    productStockList.Add(new ProductWithStockViewModel { ProductID = product.ProductID, Name = product.Name, ProductStockID = stock.ProductStockID, Booked = stock.Booked, InStock = stock.InStock });
                }
                return View(productStockList);
        }

        [HttpGet]
        public RedirectToActionResult AddToStock (int quantityToStock, int productId)
        {
            if (quantityToStock < 0)
            {
                TempData["message"] = "Incorrect quantity data!";
                return RedirectToAction("Index");
            }
            stockRepository.AddToStock(productId, quantityToStock);
            TempData["message"] = "Quantity was added!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetBrands([FromBody]PassingCategoryName value)
        {
            var brands = categoryRepository.GetBrandsByCategoryName(value.Category).Select(b => new { BrandId = b.BrandID, Category = b.Name }).ToArray();
            return Json(brands);
        }
    }
}