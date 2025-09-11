using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    // 6. Использование абстрактного класса
    // 9. Использование наследования (будет базой для других)
    // 7. Использование инкапсуляции (поля protected и private)
    public abstract class StationeryItem : ISellable
    {
        // 7. Поле защищено, доступно только в классе и наследниках
        protected string _name;

        // 8. Статическое поле для подсчета всех созданных товаров
        private static int _itemCount = 0;

        // 3. Использование свойств
        public decimal Price { get; set; }
        public string Name => _name; // Свойство только для чтения (лямбда-выражение)

        // 4. Свойство с логикой в get блоке
        public static int TotalItemsCreated => _itemCount;

        // 2. Использование конструктора с параметрами
        public StationeryItem(string name, decimal price)
        {
            _name = name;
            Price = price;
            _itemCount++; // Увеличиваем общий счетчик при создании объекта
        }

        // 6. Абстрактный метод, который наследники обязаны реализовать
        // 10. Будет переопределен в классах-наследниках
        public abstract void DisplayInfo();

        public void Sell()
        {
            Console.WriteLine($"Продан товар: '{_name}' по цене {Price:C}");
        }
    }
}
