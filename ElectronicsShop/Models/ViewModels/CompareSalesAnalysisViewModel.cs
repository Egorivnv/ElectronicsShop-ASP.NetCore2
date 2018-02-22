﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models.ViewModels
{
    public class CompareSalesAnalysisViewModel
    {
            public string Category { get; set; }
            public int Brand { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public int CartView { get; set; }
    }
}
