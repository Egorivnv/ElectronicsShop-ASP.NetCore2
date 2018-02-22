using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Controllers
{
    [Authorize]
    public class AdminOrdersController : Controller
    {
        private IOrderRepository repository;
        public AdminOrdersController(IOrderRepository repo)
        {
            repository = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowOrdersTable (int ordersState)
        {
            bool? status = null;
            if (ordersState == 1) status = true;
            if (ordersState == 2) status = false;
            if (ordersState == 3) return View(repository.Orders);
            IQueryable<Order> ordersList = repository.Orders.Where(o => o.Shipped == status).AsQueryable();
            return View(ordersList);
        }
        public IActionResult ViewOrder (int orderId)
        {
            return View(repository.Orders.Where(o => o.OrderID == orderId).FirstOrDefault());
        }
        [HttpGet]
        public IActionResult EditOrder(Order order, int status)
        {
            bool? statusRes = null;
            if (status == 1) statusRes = true;
            if (status == 2) statusRes = false;
            order.Shipped = statusRes;
            repository.SaveOrder(order);
            TempData["message"] = $"Order № {order.OrderID} has been saved";
            return RedirectToAction("Index");
        }
    }
}