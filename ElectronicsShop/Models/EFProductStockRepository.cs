using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class EFProductStockRepository : IProductStockRepository
    {
        private ApplicationDbContext context;

        public EFProductStockRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ProductStock> Stocks => context.ProductStocks.AsQueryable();

        public void AddToBooked(int productId, int quantity)
        {
            ProductStock stock = Stocks.FirstOrDefault(s => s.ProductIdent == productId);
            if (stock != null && stock.InStock >= quantity)
            {
                stock.Booked += quantity;
            }
            context.SaveChanges();
        }

        public void AddToStock(int productId, int quantity)
        {
            ProductStock stock = Stocks.FirstOrDefault(s => s.ProductIdent == productId);
            if (stock != null)
            {
                stock.InStock += quantity;
            }
            else
            {
                stock = new ProductStock { ProductIdent = productId, InStock = quantity, Booked = 0 };
                context.ProductStocks.Add(stock);
            }
            context.SaveChanges();
        }

        public void DeductFromBooked(int stockId, int quantity)
        {
            throw new NotImplementedException();
        }

        public void DeductFromStock(int stockId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
