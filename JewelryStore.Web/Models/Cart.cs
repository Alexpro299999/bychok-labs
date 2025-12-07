using JewelryStore.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace JewelryStore.Web.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = Lines.Where(p => p.Product.Id == product.Id).FirstOrDefault();
            if (line == null) Lines.Add(new CartLine { Product = product, Quantity = quantity });
            else line.Quantity += quantity;
        }
        public virtual void RemoveLine(Product product) => Lines.RemoveAll(l => l.Product.Id == product.Id);
        public decimal ComputeTotalValue() => Lines.Sum(e => e.Product.Price * e.Quantity);
        public virtual void Clear() => Lines.Clear();
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}