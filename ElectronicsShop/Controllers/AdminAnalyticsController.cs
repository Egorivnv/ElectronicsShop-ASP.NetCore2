using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models;
using ElectronicsShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ElectronicsShop.Controllers
{
    [Authorize]
    public class AdminAnalyticsController : Controller
    {
        private IProductRepository repository;
        private ICatalogRepository categoryRepository;
        private IAnalyticsCalculate analyticsCalculate;
        public AdminAnalyticsController(IProductRepository repo, ICatalogRepository catergoryRep, IAnalyticsCalculate analyticsCalc)
        {
            repository = repo;
            categoryRepository = catergoryRep;
            analyticsCalculate = analyticsCalc;
        }

        public IActionResult Index() => View();

        public IActionResult ComparativeSalesAnalytics() => View();

        [HttpPost]
        public JsonResult GetProducts([FromBody]PassingCategoryBrand value)
        {
            var products = repository.Products.Where(p => p.Category == value.Category).Where(p => p.Brand == value.BrandId).Select(b => new { ProductId = b.ProductID, ProductName = b.Name });
            return Json(products);
        }
        [HttpPost]
        public IActionResult ShowAnalysis(SalesAnalysisViewModel data)
        {
            if (data.Interval == 0) { ViewBag.ErrMessage = "The interval wasn't entered!"; return View("ErrorView"); }
            if (data.DateFrom == null || data.DateTo == null) { ViewBag.ErrMessage = "Date wasn't entered! "; return View("ErrorView"); }
            if (data.DateFrom >= data.DateTo) { ViewBag.ErrMessage = "Entered 'date from' is greater than 'date to' !"; return View("ErrorView"); }
            if (data.DateTo > DateTime.Now) { ViewBag.ErrMessage = "Entered 'date to' is gteater than current date!"; return View("ErrorView"); }

            string brand = data.Brand == 0 ? "all" : categoryRepository.Categories.FirstOrDefault(c => c.Name == data.Category).Brands.FirstOrDefault(b => b.BrandID == data.Brand).Name;
            string product = data.Product == 0 ? "all" : repository.Products.FirstOrDefault(p => p.ProductID == data.Product).Name;

            List<DateTime> dates = new List<DateTime>();
            DateTime temp = data.DateFrom;
            int interval = data.Interval;
            while (temp <= data.DateTo.AddDays(interval))
            {
                dates.Add(temp);
                temp = temp.AddDays(interval);
            };

            List<SalesAnalyticsDataViewModel> analyticsData = new List<SalesAnalyticsDataViewModel>();

            if (data.Category == "all" && data.Brand == 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesIntervaledData(dates, repository.Products));
            }

            if (data.Category != "all" && data.Brand == 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesIntervaledData(dates, repository.Products.Where(p => p.Category == data.Category)));
            }

            if (data.Category != "all" && data.Brand != 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesIntervaledData(dates, repository.Products.Where(p => p.Category == data.Category).Where(p => p.Brand == data.Brand)));
            }

            if (data.Category != "all" && data.Brand != 0 && data.Product != 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesIntervaledData(dates, repository.Products.Where(p => p.Category == data.Category).Where( p => p.ProductID == data.Product )));
            }

            ViewBag.GrafTitle = "Category: " + data.Category + ". Brand: " + brand + ". Product: " + product + ". Period: from " + data.DateFrom.ToShortDateString() + " to " + data.DateTo.ToShortDateString() + " year";
            ViewBag.TotalSales = analyticsData.Sum(d => d.y);
            

            switch (data.CartView)
            {
                case 0:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("SplineChart");
                    }
                case 1:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("StepLineChart");
                    }
                case 2:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("AreaChart");
                    }

                case 3:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("SplineAreaChart");
                    }

                case 4:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("StepAreaChart");
                    }

                default:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("SplineChart");
                    }
            }
            
        }

        public IActionResult ShowCompareAnalysis(CompareSalesAnalysisViewModel data)
        {
            if (data.DateFrom == null || data.DateTo == null) { ViewBag.ErrMessage = "Date wasn't entered! "; return View("ErrorView"); }
            if (data.DateFrom >= data.DateTo) { ViewBag.ErrMessage = "Entered 'date from' is greater than 'date to' !"; return View("ErrorView"); }
            if (data.DateTo > DateTime.Now) { ViewBag.ErrMessage = "Entered 'date to' is gteater than current date!"; return View("ErrorView"); }

            List<SalesAnalyticsDataViewModel> analyticsData = new List<SalesAnalyticsDataViewModel>();

            if (data.Category == "all" && data.Brand == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesCompareCategorysData(data.DateFrom, data.DateTo));
            }

            if (data.Category != "all" && data.Brand == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesCompareBrandsData(data.DateFrom, data.DateTo, data.Category ));
            }

            if (data.Category != "all" && data.Brand != 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetSalesCompareProductsData(data.DateFrom, data.DateTo, data.Category, data.Brand ));
            }

            string brand = data.Brand == 0 ? "all" : categoryRepository.Categories.FirstOrDefault(c => c.Name == data.Category).Brands.FirstOrDefault(b => b.BrandID == data.Brand).Name;
            ViewBag.xParam = "Category: " + data.Category + ". Brand: " + brand + ".";
            ViewBag.GrafTitle = "Category: " + data.Category + ". Brand: " + brand + ". Period: from " + data.DateFrom.ToShortDateString() + " to " + data.DateTo.ToShortDateString() + " year";
            ViewBag.TotalSales = analyticsData.Sum(d => d.y);

            switch (data.CartView)
            {
                case 0:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("ColumnChart");
                    }
                case 1:
                    {
                        var analyticsDataNew = analyticsData.Select(d => new { y = d.y, label = d.label, indexLabel = d.label });
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsDataNew, _jsonSetting);
                        return View("BarChart");
                    }
                case 2:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("PieChart");
                    }
                case 3:
                    {
                        var analyticsDataNew = analyticsData.Select(d => new { y = d.y, indexLabel = d.label });
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsDataNew, _jsonSetting);
                        return View("PyramidChart");
                    }
                default:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("ColumnChart");
                    }
            };

        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    }

    public class PassingCategoryBrand
    {
        public string Category { get; set; }
        public int BrandId { get; set; }
    }

}