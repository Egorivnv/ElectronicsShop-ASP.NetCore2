using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Models
{
    public class EFCatalogRepository : ICatalogRepository
    {
        private ApplicationDbContext context;
        public EFCatalogRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Category> Categories => context.Categories.Include(o => o.Brands);

        public IQueryable<Brand> GetBrandsByCategoryName(string categoryName)
        {
            return context.Categories.Include(o => o.Brands).Where(c => c.Name == categoryName).FirstOrDefault().Brands.AsQueryable();
        }
    }
}
