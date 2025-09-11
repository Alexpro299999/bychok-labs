using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 1. Один из 4+ собственных классов
    // 9. Наследование от StationeryItem
    public class Pen : StationeryItem
    {
        public string InkColor { get; set; }

        // 17. Использование композиции.
        // Стержень является неотъемлемой частью ручки и управляется ею.
        private InkCartridge _cartridge;

        public Pen(string name, decimal price, string inkColor) : base(name, price)
        {
            InkColor = inkColor;
            // Объект InkCartridge создается внутри Pen и не может существовать без него
            _cartridge = new InkCartridge(inkColor);
        }

        public string GetCartridgeInfo()
        {
            return _cartridge.GetInfo();
        }

        // 10. Переопределение абстрактного метода
        public override void DisplayInfo()
        {
            Console.WriteLine($"Ручка: '{_name}', Цвет чернил: {InkColor}, Цена: {Price:C}");
        }
    }
}
