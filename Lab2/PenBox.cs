using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
   
    public class PenBox<T> : Box<T> where T : Pen
    {
        public List<T> GetPensByColor(string color)
        {
            List<T> foundPens = _itemsInBox
                .Where(pen => pen.InkColor.Equals(color, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return foundPens;
        }
    }
}