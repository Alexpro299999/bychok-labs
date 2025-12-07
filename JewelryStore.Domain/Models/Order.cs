using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JewelryStore.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}