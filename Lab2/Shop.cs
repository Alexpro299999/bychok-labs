using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Shop
    {
        private List<StationeryItem> _items = new List<StationeryItem>();

        // 16. Использование агрегации.
        // Магазин "имеет" список сотрудников, но они могут существовать и без магазина.
        private List<Employee> _employees = new List<Employee>();

        public string Name { get; private set; }

        public Shop(string name)
        {
            Name = name;
        }

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        // 5. Использование индексатора для доступа к товарам по индексу
        public StationeryItem this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int ItemsCount => _items.Count;

        // 11. Использование перегруженного оператора (+) для добавления товара в магазин
        public static Shop operator +(Shop shop, StationeryItem item)
        {
            shop._items.Add(item);
            return shop;
        }

        // 13. Использование обобщенного метода для получения товаров определенного типа
        public T[] GetItemsOfType<T>() where T : StationeryItem
        {
            return _items.OfType<T>().ToArray();
        }

        public void PrintAllItems()
        {
            Console.WriteLine($"Товары в магазине '{Name}':");
            foreach (var item in _items)
            {
                item.DisplayInfo();
            }
        }
    }
}
