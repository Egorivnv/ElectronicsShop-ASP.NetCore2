using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public interface ICatalogRepository
    {
        IQueryable<Category> Categories { get; }
        IQueryable<Brand> GetBrandsByCategoryName(string categoryName);

    }
}
