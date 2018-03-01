using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models.ViewModels
{
    public class SupplyPlanningViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int InStock { get; set; }
        public int? StockLevelForSupply { get; set; }
        public int? NormOfSupplying { get; set; }
        public int? TimeToFormSupply { get; set; }
    }
}
