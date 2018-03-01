using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicsShop.Models;
using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Controllers
{
    [Authorize]
    public class AdminSupplyPlanningController : Controller
    {
        private IProductRepository productRepository;
        private ISupplyProduct supplyRepository;
        private IProductStockRepository stockRepository;
        private IAnalyticsCalculate analyticsCalculate;
        public AdminSupplyPlanningController(IProductRepository repos, ISupplyProduct supplyRepos, IProductStockRepository stockRepos, IAnalyticsCalculate analyticsCalc)
        {
            productRepository = repos;
            supplyRepository = supplyRepos;
            stockRepository = stockRepos;
            analyticsCalculate = analyticsCalc;
        }

        public ViewResult Index() => View();

        [HttpGet]
        public ViewResult ShowSupplyTable(string category, int brand, int status)
        {
            IQueryable<Product> productList = null;
            if (category == "all") productList = productRepository.Products;
            if (category != "all" && brand == 1) productList = productRepository.Products.Where(p => p.Category == category);
            if (category != "all" && brand != 1) productList = productRepository.Products.Where(p => p.Category == category).Where(p => p.Brand == brand);

            List<SupplyPlanningViewModel> supplyList = new List<SupplyPlanningViewModel>();
            switch (status)
            {
                case 0:
                    { 
                        foreach (var product in productList)
                        {
                            var supplyProductParameter = supplyRepository.SupplyProductParameters.FirstOrDefault(s => s.Product == product);
                            if (supplyProductParameter != null)
                            {
                                supplyList.Add(new SupplyPlanningViewModel
                                {
                                    ProductId = supplyProductParameter.Product.ProductID,
                                    InStock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID).InStock,
                                    ProductName = product.Name,
                                    TimeToFormSupply = supplyProductParameter.TimeToFormSupply,
                                    NormOfSupplying = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.SupplyFrequency, product, supplyProductParameter.SafetyRatio),
                                    StockLevelForSupply = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.TimeToFormSupply, product, supplyProductParameter.SafetyRatio)
                            });
                            }
                            else
                            {
                                supplyList.Add(new SupplyPlanningViewModel
                                {
                                    ProductId = product.ProductID,
                                    InStock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID)?.InStock ?? 0,
                                    ProductName = product.Name,
                                    TimeToFormSupply = null,
                                    NormOfSupplying = null,
                                    StockLevelForSupply = null
                                });
                            }

                        }
                    }; break;

                case 1:
                    {
                        foreach (var product in productList)
                        {
                            var supplyProductParameter = supplyRepository.SupplyProductParameters.FirstOrDefault(s => s.Product == product);
                            if (supplyProductParameter != null)
                            {
                                int stock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID).InStock;
                                int tempStockLevelForSupply = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.TimeToFormSupply, product, supplyProductParameter.SafetyRatio);
                                if (stock <= tempStockLevelForSupply && stock >= tempStockLevelForSupply - supplyProductParameter.TimeToFormSupply) {
                                    supplyList.Add(new SupplyPlanningViewModel
                                    {
                                        ProductId = supplyProductParameter.Product.ProductID,
                                        InStock = stock,
                                        ProductName = product.Name,
                                        TimeToFormSupply = supplyProductParameter.TimeToFormSupply,
                                        NormOfSupplying = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.SupplyFrequency, product, supplyProductParameter.SafetyRatio),
                                        StockLevelForSupply = tempStockLevelForSupply,
                                    });
                                }
                            }
                        }
                    }; break;

                case 2:
                    {
                        foreach (var product in productList)
                        {
                            var supplyProductParameter = supplyRepository.SupplyProductParameters.FirstOrDefault(s => s.Product == product);
                            if (supplyProductParameter != null)
                            {
                                int stock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID).InStock;
                                int tempStockLevelForSupply = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.TimeToFormSupply, product, supplyProductParameter.SafetyRatio);
                                if (stock < tempStockLevelForSupply - supplyProductParameter.TimeToFormSupply)
                                {
                                    supplyList.Add(new SupplyPlanningViewModel
                                    {
                                        ProductId = supplyProductParameter.Product.ProductID,
                                        InStock = stock,
                                        ProductName = product.Name,
                                        TimeToFormSupply = supplyProductParameter.TimeToFormSupply,
                                        NormOfSupplying = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.SupplyFrequency, product, supplyProductParameter.SafetyRatio),
                                        StockLevelForSupply = tempStockLevelForSupply,
                                    });
                                }
                            }
                        }
                    }; break;
                case 3:
                    {
                        foreach (var product in productList)
                        {
                            var supplyProductParameter = supplyRepository.SupplyProductParameters.FirstOrDefault(s => s.Product == product);
                            if (supplyProductParameter != null)
                            {
                                int stock = stockRepository.Stocks.FirstOrDefault(s => s.ProductIdent == product.ProductID).InStock;
                                int tempStockLevelForSupply = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.TimeToFormSupply, product, supplyProductParameter.SafetyRatio);
                                if (stock > tempStockLevelForSupply)
                                {
                                    supplyList.Add(new SupplyPlanningViewModel
                                    {
                                        ProductId = supplyProductParameter.Product.ProductID,
                                        InStock = stock,
                                        ProductName = product.Name,
                                        TimeToFormSupply = supplyProductParameter.TimeToFormSupply,
                                        NormOfSupplying = analyticsCalculate.GetForecastSupplyDays(supplyProductParameter.SupplyFrequency, product, supplyProductParameter.SafetyRatio),
                                        StockLevelForSupply = tempStockLevelForSupply,
                                    });
                                }
                            }
                        }
                    }; break;
                default:break;
            }


            return View(supplyList);
        }

        [HttpGet]
        public IActionResult SetSupplyOptions(int productId)
        {
            SupplyProductParameter parameter = supplyRepository.SupplyProductParameters.FirstOrDefault(s => s.Product.ProductID == productId);
            if (parameter == null)
            {
                parameter = new SupplyProductParameter
                {
                    Product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId),
                };
            }
            else
            {
                parameter.Product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
            }
            return View(parameter);
        }

        [HttpPost]
        public RedirectToActionResult SetSupplyOptions(SupplyProductParameter parameter, int productId)
        {
            try
            {
                var product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
                parameter.Product = product;
                supplyRepository.SaveParameter(parameter);
            }
            catch
            {
                TempData["message"] = "Error: setting option hasn't been changed!";
                return RedirectToAction(nameof(Index));
            }
            TempData["message"] = "Setting option's been changed!";
            return RedirectToAction(nameof(Index));
        }
    }
}