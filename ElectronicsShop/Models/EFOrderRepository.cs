using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace ElectronicsShop.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);


        public bool CheckQuantity(IEnumerable<CartLine> cartLines)
        {
            int enought = 0;
            int entire = cartLines.Count();
            foreach (var line in cartLines)
            {
                ProductStock ps = context.ProductStocks.FirstOrDefault(t => t.ProductIdent == line.Product.ProductID);
                int booked = ps != null ? ps.Booked : 0;
                int available = (ps != null ? ps.InStock : 0) - booked;
                if (line.Quantity <= available) enought++;
            }
            return enought == entire ? true : false;
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderID == 0)
            {
                foreach (var line in order.Lines)
                {
                    context.ProductStocks.FirstOrDefault(t => t.ProductIdent == line.Product.ProductID).Booked += line.Quantity;
                }
                context.AttachRange(order.Lines.Select(l => l.Product));
                context.Orders.Add(order);
            } else
            {
                Order orderEdit = Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
                orderEdit.Shipped = order.Shipped;
                if (orderEdit.Shipped == true)
                {
                    foreach (var line in orderEdit.Lines)
                    {
                        context.ProductStocks.FirstOrDefault(t => t.ProductIdent == line.Product.ProductID).Booked -= line.Quantity;
                        context.ProductStocks.FirstOrDefault(t => t.ProductIdent == line.Product.ProductID).InStock -= line.Quantity;
                    }
                }
                if (orderEdit.Shipped == false)
                {
                    foreach (var line in orderEdit.Lines)
                    {
                        context.ProductStocks.FirstOrDefault(t => t.ProductIdent == line.Product.ProductID).Booked -= line.Quantity;
                    }
                }
            }
            context.SaveChanges();
        }
    }
}