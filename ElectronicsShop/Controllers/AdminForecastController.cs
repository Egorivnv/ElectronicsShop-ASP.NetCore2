using System;
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
    public class AdminForecastController : Controller
    {
        private IProductRepository repository;
        private ICatalogRepository categoryRepository;
        private IAnalyticsCalculate analyticsCalculate;
        public AdminForecastController(IProductRepository repo, ICatalogRepository catergoryRep, IAnalyticsCalculate analyticsCalc)
        {
            repository = repo;
            categoryRepository = catergoryRep;
            analyticsCalculate = analyticsCalc;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowForecast(ForecastInputDate data)
        {
            if (data.BaseInterval < 2) { ViewBag.ErrMessage = "Base interval can't be less then 2 days!"; return View("ErrorView"); }
            if (data.ForecastInterval < 1) { ViewBag.ErrMessage = "Forecast interval can't be less then 1 day!"; return View("ErrorView"); }

            int intervalLength = 0;
            switch (data.BasePeriod)
            {
                case 0: intervalLength = 1; break;
                case 1: intervalLength = 7; break;
                case 2: intervalLength = 30; break;
                case 3: intervalLength = 365; break;
                default: intervalLength = 1; break;
            }

            List<DateTime> datesBase = new List<DateTime>();
            List<DateTime> datesForecast = new List<DateTime>();

            switch (intervalLength)
            {
                case 1:
                case 7:
                    {
                        DateTime tempBase = DateTime.Now.AddDays(intervalLength * data.BaseInterval * (-1));
                        while (tempBase <= DateTime.Now)
                        {
                            datesBase.Add(tempBase);
                            tempBase = tempBase.AddDays(intervalLength);
                        };

                        DateTime tempForecast = DateTime.Now;
                        while (tempForecast <= DateTime.Now.AddDays(intervalLength * data.ForecastInterval))
                        {
                            datesForecast.Add(tempForecast);
                            tempForecast = tempForecast.AddDays(intervalLength);
                        };
                    };break;
                case 30:
                    {
                        DateTime tempBase = DateTime.Now.AddMonths(data.BaseInterval * (-1));
                        while (tempBase <= DateTime.Now)
                        {
                            datesBase.Add(tempBase);
                            tempBase = tempBase.AddMonths(1);
                        };

                        DateTime tempForecast = DateTime.Now;
                        while (tempForecast <= DateTime.Now.AddMonths(data.ForecastInterval))
                        {
                            datesForecast.Add(tempForecast);
                            tempForecast = tempForecast.AddMonths(1);
                        };
                    };
                    break;
                case 365:
                    {
                        DateTime tempBase = DateTime.Now.AddYears(data.BaseInterval * (-1));
                        while (tempBase <= DateTime.Now)
                        {
                            datesBase.Add(tempBase);
                            tempBase = tempBase.AddYears(1);
                        };

                        DateTime tempForecast = DateTime.Now;
                        while (tempForecast <= DateTime.Now.AddYears(data.ForecastInterval))
                        {
                            datesForecast.Add(tempForecast);
                            tempForecast = tempForecast.AddYears(1);
                        };
                    }; break;
                default: break;
            }

            List<SalesAnalyticsDataViewModel> analyticsData = new List<SalesAnalyticsDataViewModel>();

            if (data.Category == "all" && data.Brand == 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetForecastData(datesBase, datesForecast, repository.Products));
            }

            if (data.Category != "all" && data.Brand == 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetForecastData(datesBase, datesForecast, repository.Products.Where(p => p.Category == data.Category)));
            }

            if (data.Category != "all" && data.Brand != 0 && data.Product == 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetForecastData(datesBase, datesForecast, repository.Products.Where(p => p.Category == data.Category).Where(p => p.Brand == data.Brand)));
            }

            if (data.Category != "all" && data.Brand != 0 && data.Product != 0)
            {
                analyticsData.AddRange(analyticsCalculate.GetForecastData(datesBase, datesForecast, repository.Products.Where(p => p.Category == data.Category).Where(p => p.ProductID == data.Product)));
            }

            string brand = data.Brand == 0 ? "all" : categoryRepository.Categories.FirstOrDefault(c => c.Name == data.Category).Brands.FirstOrDefault(b => b.BrandID == data.Brand).Name;
            string product = data.Product == 0 ? "all" : repository.Products.FirstOrDefault(p => p.ProductID == data.Product).Name;

            ViewBag.GrafTitle = "Category: " + data.Category + ". Brand: " + brand + ". Product: " + product + ". Period: from " + DateTime.Now.ToShortDateString() + " in " + data.ForecastInterval + " periods" + "(" +
                (intervalLength == 1 ? "days" : intervalLength == 7 ? "weeks" : intervalLength == 30 ? "months" : intervalLength == 360 ? "years" : "") 
                + ")";
            ViewBag.TotalSales = analyticsData.Sum(d => d.y);


            switch (data.ForecastPresentation)
            {
                case 0:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("SplineChart");
                    }
                case 1:
                    {
                        ViewBag.DataRow = JsonConvert.SerializeObject(analyticsData, _jsonSetting);
                        return View("AreaChart");
                    }
                case 2:
                    {
                        return View("TableView", analyticsData);
                    }

                default:
                    {
                        return View("TableView", analyticsData);
                    }
            }
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
    }
}