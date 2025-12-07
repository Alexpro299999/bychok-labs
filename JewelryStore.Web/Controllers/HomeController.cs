using JewelryStore.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JewelryStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repo;
        public HomeController(IStoreRepository repo) => _repo = repo;

        public IActionResult Index(string category, string search, int page = 1)
        {
            int pageSize = 4;
            var query = _repo.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category)) query = query.Where(p => p.Category.Name == category);
            if (!string.IsNullOrEmpty(search)) query = query.Where(p => p.Name.Contains(search));

            var count = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)count / pageSize);
            ViewBag.CurrentCategory = category;
            ViewBag.Search = search;

            return View(items);
        }

        public IActionResult Details(int id) => View(_repo.Products.FirstOrDefault(p => p.Id == id));
    }
}