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
            IQueryable<Product> productList = null;
            if (category == "all") productList =  repository.Products;
            else if (category != "all" && brand == 0) productList = repository.Products.Where(p => p.Category == category);
            else productList = repository.Products.Where(p => p.Category == category).Where(p => p.Brand == brand);
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
                    using (var binaryReader = new BinaryReader(imageUpload.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)imageUpload.Length);
                    }
                    product.Image = imageData;
                }
                repository.SaveProduct(product);
                TempData["message"] = $"'{product.Name}' has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // if there is something wrong with the data values
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