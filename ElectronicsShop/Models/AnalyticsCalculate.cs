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
                    resultList.Add(new SalesAnalyticsDataViewModel { y = totalIntervalSum, label = temp.ToShortDateString() + "-" + dateItem.ToShortDateString() });
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
    }
}
