using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models.ViewModels
{
    public class ProductWithStockViewModel
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        //stock props
        public int ProductStockID { get; set; }
        public int InStock { get; set; }
        public int Booked { get; set; }
    }
}
