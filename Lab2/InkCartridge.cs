using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 1. Третий из 4+ собственных классов
    // Используется для демонстрации композиции в классе Pen
    public class InkCartridge
    {
        public string Color { get; private set; }

        public InkCartridge(string color)
        {
            Color = color;
        }

        public string GetInfo()
        {
            return $"Стержень с цветом: {Color}";
        }
    }
}
