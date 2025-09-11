using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Box<T> where T : StationeryItem
    {
        protected List<T> _itemsInBox = new List<T>();

        public void Add(T item)
        {
            _itemsInBox.Add(item);
            Console.WriteLine($"В коробку добавлен: {item.Name}");
        }

        public T Get(int index)
        {
            return _itemsInBox[index];
        }
    }
}
