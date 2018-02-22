using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public interface IProductStockRepository
    {
        IQueryable<ProductStock> Stocks { get; }
        void AddToStock(int stockId, int quantity);
        void AddToBooked(int stockId, int quantity);
        void DeductFromStock(int stockId, int quantity);
        void DeductFromBooked(int stockId, int quantity);
    }
}
