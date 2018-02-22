using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models.ViewModels
{
    public class AddImageViewModel
    {
        public Product Product { get; set; }
        public IFormFile FormFileImage { get; set; }
    }
}
