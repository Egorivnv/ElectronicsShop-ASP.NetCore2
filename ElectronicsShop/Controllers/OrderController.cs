using Microsoft.AspNetCore.Mvc;
using ElectronicsShop.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ElectronicsShop.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !(bool)o.Shipped));

        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult CheckoutShow() => View("Checkout", new Order());

        public RedirectToActionResult Checkout()
        {
            if (repository.CheckQuantity(cart.Lines) == true) return RedirectToAction(nameof(CheckoutShow));
            else return RedirectToAction(nameof(LackOfQuantity));
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                order.Date = DateTime.Now;
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));

            }
            else
            {
                return View(order);
            }
        }
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
        public ViewResult LackOfQuantity()
        {
            cart.Clear();
            return View();
        }
    }
}