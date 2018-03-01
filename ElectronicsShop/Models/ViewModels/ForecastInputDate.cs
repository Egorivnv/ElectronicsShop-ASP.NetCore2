using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models.ViewModels
{
    public class ForecastInputDate
    {
        public string Category { get; set; }
        public int Brand { get; set; }
        public int Product { get; set; }
        public int BasePeriod { get; set; }
        public int BaseInterval { get; set; }
        public int ForecastInterval { get; set; }
        public int ForecastPresentation { get; set; }
    }
}
