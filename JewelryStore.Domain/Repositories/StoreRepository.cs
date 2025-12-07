using JewelryStore.Domain.Data;
using JewelryStore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JewelryStore.Domain.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext _context;
        public StoreRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products.Include(p => p.Category);
        public IQueryable<Category> Categories => _context.Categories;
        public IQueryable<Order> Orders => _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product);

        public void SaveProduct(Product product)
        {
            if (product.Id == 0) _context.Products.Add(product);
            else _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == productId);
            if (p != null)
            {
                _context.Products.Remove(p);
                _context.SaveChanges();
            }
        }

        public void SaveOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
        }
    }
}