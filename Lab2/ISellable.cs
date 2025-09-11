using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 18. Определение собственного интерфейса
    public interface ISellable
    {
        decimal Price { get; set; }
        string Name { get; }
        void Sell();
    }
}
