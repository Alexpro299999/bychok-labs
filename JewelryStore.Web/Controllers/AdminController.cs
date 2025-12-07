using JewelryStore.Domain.Models;
using JewelryStore.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JewelryStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IStoreRepository _repo;
        public AdminController(IStoreRepository repo) => _repo = repo;

        public IActionResult Index() => View(_repo.Products);
        public IActionResult Orders() => View(_repo.Orders);

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_repo.Categories, "Id", "Name");

            var product = _repo.Products.FirstOrDefault(p => p.Id == id) ?? new Product();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            ModelState.Remove("Category");
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
                    using (var stream = new FileStream(path, FileMode.Create)) await image.CopyToAsync(stream);
                    product.ImageUrl = "/images/" + image.FileName;
                }

                _repo.SaveProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_repo.Categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            _repo.UpdateOrderStatus(orderId, status);
            return RedirectToAction("Orders");
        }
    }
}