using JewelryStore.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace JewelryStore.Domain.Repositories
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Order> Orders { get; }
        void SaveProduct(Product product);
        void DeleteProduct(int productId);
        void SaveOrder(Order order);
        void UpdateOrderStatus(int orderId, string status);
    }
}