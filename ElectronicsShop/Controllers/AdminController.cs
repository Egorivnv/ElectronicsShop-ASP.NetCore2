using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ElectronicsShop.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ElectronicsShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;
        private ICatalogRepository categoryRepository;
        public AdminController(IProductRepository repo, ICatalogRepository catergoryRep)
        {
            repository = repo;
            categoryRepository = catergoryRep;
        }

        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult ShowProductTable(string category, int brand)
        {
            if (categoryRepository.Categories.Where(c => c.Name == category).FirstOrDefault() == null) return View(repository.Products);
            IQueryable<Product> productList = repository.Products.Where(p => p.Category == category).Where(p => p.Brand == brand).AsQueryable();
            return View(productList);
        }
        public ViewResult Edit(int productId) => View(repository.Products.FirstOrDefault(p => p.ProductID == productId));

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile imageUpload = null)
        {
            if (ModelState.IsValid)
            {
                if (imageUpload != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(imageUpload.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)imageUpload.Length);
                    }
                    // установка массива байтов
                    product.Image = imageData;
                }
                repository.SaveProduct(product);
                TempData["message"] = $"'{product.Name}' has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }
        public ViewResult Create() => View("Edit", new Product());
        [HttpPost]
        public IActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }
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