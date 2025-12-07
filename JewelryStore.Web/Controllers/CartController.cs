using JewelryStore.Domain.Repositories;
using JewelryStore.Web.Infrastructure;
using JewelryStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JewelryStore.Web.Controllers
{
    public class CartController : Controller
    {
        private IStoreRepository _repo;
        public CartController(IStoreRepository repo) => _repo = repo;

        public Cart GetCart() => HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        public void SaveCart(Cart cart) => HttpContext.Session.SetJson("Cart", cart);

        public IActionResult Index() => View(GetCart());

        public IActionResult AddToCart(int productId, string returnUrl)
        {
            var product = _repo.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var product = _repo.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
    }
}