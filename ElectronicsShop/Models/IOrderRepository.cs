using System.Collections.Generic;
using System.Linq;
namespace ElectronicsShop.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        bool CheckQuantity(IEnumerable<CartLine> cartLine);
    }
}