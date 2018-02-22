using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class ProductStock
    {
        public int ProductStockID { get; set; }
        public int InStock { get; set; }
        public int Booked { get; set; }
        public int ProductIdent { get; set; }
    }
}
