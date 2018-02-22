using ElectronicsShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public interface IAnalyticsCalculate
    {
        IEnumerable<SalesAnalyticsDataViewModel> GetSalesIntervaledData(IEnumerable<DateTime> dateList, IQueryable<Product> productList);

        IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareCategorysData(DateTime dateFrom, DateTime dateTo);
        IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareBrandsData(DateTime dateFrom, DateTime dateTo, string category);
        IEnumerable<SalesAnalyticsDataViewModel> GetSalesCompareProductsData(DateTime dateFrom, DateTime dateTo, string category, int brandId);
    }
}
