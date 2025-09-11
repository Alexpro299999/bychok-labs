using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 1. Второй из 4+ собственных классов
    public class Notebook : StationeryItem
    {
        public int PageCount { get; set; }

        public Notebook(string name, decimal price, int pageCount) : base(name, price)
        {
            // 4. Свойство с логикой в set блоке (валидация)
            if (pageCount <= 0)
            {
                throw new ArgumentException("Количество страниц должно быть положительным.");
            }
            PageCount = pageCount;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Блокнот: '{_name}', Кол-во страниц: {PageCount}, Цена: {Price:C}");
        }
    }
}
