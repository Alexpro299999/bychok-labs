using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 // <--- Убедитесь, что эта строка есть
{
    public static class StationeryExtensions
    {
        public static decimal GetDiscountedPrice(this ISellable item, int percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentException("Скидка должна быть в диапазоне от 0 до 100.");
            }
            return item.Price * (1 - percent / 100.0m);
        }
    }
}
