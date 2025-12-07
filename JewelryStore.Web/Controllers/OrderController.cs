using JewelryStore.Domain.Models;
using JewelryStore.Domain.Repositories;
using JewelryStore.Web.Infrastructure;
using JewelryStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JewelryStore.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IStoreRepository _repo;
        private UserManager<IdentityUser> _userManager;

        public OrderController(IStoreRepository repo, UserManager<IdentityUser> usr)
        {
            _repo = repo;
            _userManager = usr;
        }

        public IActionResult Checkout() => View();

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("Status");
            ModelState.Remove("OrderItems");

            var cart = HttpContext.Session.GetJson<Cart>("Cart");
            if (cart == null || cart.Lines.Count == 0) ModelState.AddModelError("", "Корзина пуста");

            if (ModelState.IsValid)
            {
                order.UserId = _userManager.GetUserId(User);
                order.OrderDate = DateTime.Now;
                order.Status = "Принят";
                order.OrderItems = cart.Lines.Select(l => new OrderItem
                {
                    ProductId = l.Product.Id,
                    Quantity = l.Quantity,
                    Price = l.Product.Price
                }).ToList();

                _repo.SaveOrder(order);
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Completed");
            }
            return View(order);
        }
        public IActionResult Completed() => View();

        public IActionResult History()
        {
            var userId = _userManager.GetUserId(User);
            return View(_repo.Orders.Where(o => o.UserId == userId));
        }
    }
}