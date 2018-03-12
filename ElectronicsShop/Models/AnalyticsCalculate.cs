using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models.ViewModels;

namespace ElectronicsShop.Models
{
    public class AnalyticsCalculate : IAnalyticsCalculate
    {
        private IOrderRepository orderRepository;
        private ICatalogRepository catalogRepository;
        private IProductRepository productRepository;
        public AnalyticsCalculate(IOrderRepository orderRepos, ICatalogRepository catalogRepos, IProductRepository productRepos)
        {
            orderRepository = orderRepos;
            catalogRepository = catalogRepos;
            productRepository = productRepos;
        }

        public IEnumerable<SalesAnalyticsDataViewModel> GetSalesIntervaledData(IEnumerable<DateTime> dateList, IQueryable<Product> productList)
        {
            List<SalesAnalyticsDataViewModel> resultList = new List<SalesAnalyticsDataViewModel>();
            DateTime temp = dateList.FirstOrDefault();
            foreach (var dateItem in dateList)
            {
                if (dateItem != temp)
                {
                    decimal totalIntervalSum = 0M;
                    List<Order> ordersFilterd = new List<Order>();
                    ordersFilterd.AddRange(orderRepository.Orders.Where(o => o.Date >= temp && o.Date < dateItem && o.Shipped == true));
                    foreach (var product in productList)
                    {
                        var tempLineList  = ordersFilterd.SelectMany(o => o.Lines).Where(l => l.Product.ProductID == product.ProductID);
                        totalIntervalSum += tempLineList == null ? 0M : tempLineList.Sum(l => l.Quantity * l.Product.Price);
                    }
                    resultList.Add(new SalesAnalyticsDataViewModel { y = totalIntervalSum, label = temp.ToShortDateString()  });
                    temp = dateItem;
                }
            }

            return resultList;
        }

        public IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareCategorysData(DateTime dateFrom, DateTime dateTo)
        {
            List<SalesAnalyticsDataViewModel> resultList = new List<SalesAnalyticsDataViewModel>();
            foreach (var categoryName in catalogRepository.Categories.Select(c=> c.Name))
            {
                List<Order> ordersFiltered = orderRepository.Orders.Where(o => o.Date >= dateFrom && o.Date <= dateTo && o.Shipped == true).ToList();
                var tempLineList = ordersFiltered.SelectMany(o => o.Lines).Where(l => l.Product.Category == categoryName);
                decimal totalCategorySum = tempLineList == null ? 0M : tempLineList.Sum(l => l.Quantity * l.Product.Price);

                resultList.Add(new SalesAnalyticsDataViewModel { y = totalCategorySum, label = categoryName });
            }

            return resultList;
        }


        public IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareBrandsData(DateTime dateFrom, DateTime dateTo, string category)
        {
            List<SalesAnalyticsDataViewModel> resultList = new List<SalesAnalyticsDataViewModel>();
            var brands = catalogRepository.Categories.FirstOrDefault(c => c.Name == category).Brands;
            foreach (var brand in brands)
            {
                List<Order> ordersFiltered = orderRepository.Orders.Where(o => o.Date >= dateFrom && o.Date <= dateTo && o.Shipped == true).ToList();
                var tempLineList = ordersFiltered.SelectMany(o => o.Lines).Where(l => l.Product.Brand == brand.BrandID);
                decimal totalCategorySum = tempLineList == null ? 0M : tempLineList.Sum(l => l.Quantity * l.Product.Price);

                resultList.Add(new SalesAnalyticsDataViewModel { y = totalCategorySum, label = brand.Name });
            }

            return resultList;
        }

        public IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareProductsData(DateTime dateFrom, DateTime dateTo, string category, int brandId)
        {
            List<SalesAnalyticsDataViewModel> resultList = new List<SalesAnalyticsDataViewModel>();
            var products = productRepository.Products.Where(p => p.Category == category && p.Brand == brandId);
            foreach (var product in products)
            {
                List<Order> ordersFiltered = orderRepository.Orders.Where(o => o.Date >= dateFrom && o.Date <= dateTo && o.Shipped == true).ToList();
                var tempLineList = ordersFiltered.SelectMany(o => o.Lines).Where(l => l.Product.ProductID == product.ProductID);
                decimal totalCategorySum = tempLineList == null ? 0M : tempLineList.Sum(l => l.Quantity * l.Product.Price);

                resultList.Add(new SalesAnalyticsDataViewModel { y = totalCategorySum, label = product.Name });
            }

            return resultList;
        }

        public IEnumerable<SalesAnalyticsDataViewModel> GetForecastData(IEnumerable<DateTime> dateListBase, IEnumerable<DateTime> dateListForecast, IQueryable<Product> productList)
        {
            // a0n + a1∑t = ∑y
            // a0∑t + a1∑t2 = ∑y•t

            int t = 0;
            int sumT = 0;
            int sumTSquared = 0;
            decimal SumYSquared = 0M;
            decimal SumY = 0M;
            decimal SumYT = 0M;

            DateTime temp = dateListBase.FirstOrDefault();
            foreach (var dateBase in dateListBase)
            {
                if (dateBase != temp)
                {

                    decimal totalIntervalSum = 0M;
                    List<Order> ordersFilterd = new List<Order>();
                    ordersFilterd.AddRange(orderRepository.Orders.Where(o => o.Date >= temp && o.Date < dateBase && o.Shipped == true));
                    foreach (var product in productList)
                    {
                        var tempLineList = ordersFilterd.SelectMany(o => o.Lines).Where(l => l.Product.ProductID == product.ProductID);
                        totalIntervalSum += tempLineList == null ? 0M : tempLineList.Sum(l => l.Quantity * l.Product.Price);
                    }
                    
                    temp = dateBase;

                    //if (totalIntervalSum != 0M) // !!! For using with test data
                    //{
                        t++;
                        sumT += t;
                        sumTSquared += t * t;
                        SumY += totalIntervalSum;
                        SumYT += totalIntervalSum * t;
                        SumYSquared += totalIntervalSum * totalIntervalSum;
                    //}
                }
            }


            decimal a0 = ((decimal)t * SumYT - (decimal)sumT * SumY) / (decimal)(t * sumTSquared - sumT * sumT);
            decimal a1 = (SumY - a0 * (decimal)sumT) / (decimal)t;

            List<SalesAnalyticsDataViewModel> resultList = new List<SalesAnalyticsDataViewModel>();

            temp = dateListForecast.FirstOrDefault();
            int tForecast = t + 1;
            foreach (var dateForecast in dateListForecast)
            {
                if (dateForecast != temp)
                {
                    decimal y = a0 * (decimal)tForecast + a1;
                    decimal yResult = y < 0 ? 0 : y;
                    resultList.Add(new SalesAnalyticsDataViewModel { label = temp.ToShortDateString() + "-" + dateForecast.ToShortDateString(), y = yResult });
                    temp = dateForecast;
                    tForecast++;
                }
            }
            return resultList;
        }

        public int GetForecastSupplyDays(int period, Product product, int ratio)
        {
            List<DateTime> datesBase = new List<DateTime>();
            List<DateTime> datesForecast = new List<DateTime>();

            DateTime tempBase = DateTime.Now.AddDays(period * 3 * (-1));
            while (tempBase <= DateTime.Now)
            {
                datesBase.Add(tempBase);
                tempBase = tempBase.AddDays(1);
            };

            DateTime tempForecast = DateTime.Now;
            while (tempForecast <= DateTime.Now.AddDays(period))
            {
                datesForecast.Add(tempForecast);
                tempForecast = tempForecast.AddDays(1);
            };
            List<Product> productDate = new List<Product> { product };
            var forecast = GetForecastData(datesBase, datesForecast, productDate.AsQueryable());

            return (int)Math.Ceiling(forecast.Sum(f => f.y) / product.Price * ((decimal)ratio / 100 + 1));

        }
    }
}
