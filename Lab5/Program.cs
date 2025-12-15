using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; } 
        public string ManufacturerName { get; set; } 

        public override string ToString() => $"{Name} ({ManufacturerName}) - {Price:C}";
    }

    public class Manufacturer
    {
        public string Name { get; set; } 
        public string Country { get; set; }
        public int YearFounded { get; set; }

        public override string ToString() => $"{Name} ({Country}, основан в {YearFounded})";
    }


    class Program
    {
        //для красивого вывода коллекций
        static void PrintCollection<T>(string title, IEnumerable<T> collection)
        {
            Console.WriteLine($"--- {title} ---");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Name = "Apple", Country = "США", YearFounded = 1976 },
                new Manufacturer { Name = "Samsung", Country = "Южная Корея", YearFounded = 1938 },
                new Manufacturer { Name = "Logitech", Country = "Швейцария", YearFounded = 1981 },
                new Manufacturer { Name = "Xiaomi", Country = "Китай", YearFounded = 2010 }
            };

            List<Product> products = new List<Product>
            {
                new Product { Name = "iPhone 15", Price = 1200, ManufacturerName = "Apple" },
                new Product { Name = "MacBook Pro", Price = 2500, ManufacturerName = "Apple" },
                new Product { Name = "Galaxy S24", Price = 1100, ManufacturerName = "Samsung" },
                new Product { Name = "QLED TV", Price = 1800, ManufacturerName = "Samsung" },
                new Product { Name = "MX Master 3S Mouse", Price = 100, ManufacturerName = "Logitech" },
                new Product { Name = "G Pro X Keyboard", Price = 150, ManufacturerName = "Logitech" },
                new Product { Name = "Redmi Note 13", Price = 300, ManufacturerName = "Xiaomi" },
                new Product { Name = "Mi Band 8", Price = 40, ManufacturerName = "Xiaomi" }
            };

            Console.WriteLine("===== Лабораторная работа 5: LINQ =====");
            PrintCollection("Исходный список производителей:", manufacturers);
            PrintCollection("Исходный список продуктов:", products);


            // 1. Фильтрация по одному критерию (Метод расширения)
            var cheapProducts = products.Where(p => p.Price < 200);
            PrintCollection("1. Продукты дешевле $200:", cheapProducts);

            // 2. Фильтрация по двум критериям, один от пользователя (Запрос LINQ)
            Console.Write("2. Введите производителя для поиска дорогих продуктов (e.g., Apple, Samsung): ");
            string manufacturerInput = Console.ReadLine();
            var expensiveProductsByManufacturer = from p in products
                                                  where p.ManufacturerName == manufacturerInput && p.Price > 1000
                                                  select p;
            PrintCollection($"2. Дорогие (> $1000) продукты от '{manufacturerInput}':", expensiveProductsByManufacturer);

            // 3. Сортировка (Запрос LINQ)
            var sortedByPrice = from p in products
                                orderby p.Price descending
                                select p;
            PrintCollection("3. Продукты, отсортированные по убыванию цены:", sortedByPrice);

            // 4. Размер выборки (Метод расширения Count)
            int xiaomiProductCount = products.Count(p => p.ManufacturerName == "Xiaomi");
            Console.WriteLine("--- 4. Количество продуктов от Xiaomi ---");
            Console.WriteLine($"Найдено продуктов: {xiaomiProductCount}\n");

            // 5. Агрегирующие функции (Методы расширения Max, Average, Sum)
            decimal maxPrice = products.Max(p => p.Price);
            decimal avgPrice = products.Average(p => p.Price);
            decimal sumPrice = products.Sum(p => p.Price);
            Console.WriteLine("--- 5. Агрегирующие функции по цене ---");
            Console.WriteLine($"Максимальная цена: {maxPrice:C}");
            Console.WriteLine($"Средняя цена: {avgPrice:C}");
            Console.WriteLine($"Общая стоимость всех продуктов: {sumPrice:C}\n");

            // 6. Оператор let (Запрос LINQ)
            var manufacturerAges = from m in manufacturers
                                   let age = DateTime.Now.Year - m.YearFounded
                                   select new { ManufacturerName = m.Name, Age = age };
            Console.WriteLine("--- 6. Возраст каждой компании (оператор let) ---");
            foreach (var item in manufacturerAges)
            {
                Console.WriteLine($"Компании {item.ManufacturerName} - {item.Age} лет");
            }
            Console.WriteLine();

            // 7. Группировка по одному списку (Запрос LINQ)
            var productsByManufacturer = from p in products
                                         group p by p.ManufacturerName;
            Console.WriteLine("--- 7. Группировка продуктов по производителю ---");
            foreach (var group in productsByManufacturer)
            {
                Console.WriteLine($"Производитель: {group.Key}");
                foreach (var product in group)
                {
                    Console.WriteLine($"  - {product.Name}");
                }
            }
            Console.WriteLine();

            // 8. Соединение двух списков (Метод расширения Join)
            var productWithCountry = products.Join(manufacturers,
                                                  p => p.ManufacturerName, // Внешний ключ
                                                  m => m.Name,             // Внутренний ключ
                                                  (p, m) => new { ProductName = p.Name, Country = m.Country }); // Результат
            Console.WriteLine("--- 8. Продукт и страна производителя (Join) ---");
            foreach (var item in productWithCountry)
            {
                Console.WriteLine($"{item.ProductName} - произведено в {item.Country}");
            }
            Console.WriteLine();

            // 9. Групповое соединение (Запрос LINQ с GroupJoin)
            var manufacturersWithProducts = from m in manufacturers
                                            join p in products on m.Name equals p.ManufacturerName into productGroup
                                            select new { Manufacturer = m, Products = productGroup };
            Console.WriteLine("--- 9. Список производителей со своими продуктами (GroupJoin) ---");
            foreach (var item in manufacturersWithProducts)
            {
                Console.WriteLine($"Производитель: {item.Manufacturer.Name} ({item.Products.Count()} продуктов)");
                foreach (var product in item.Products)
                {
                    Console.WriteLine($"  -> {product.Name} ({product.Price:C})");
                }
            }
            Console.WriteLine();

            // 10. Проверка условия для всех (Метод расширения All)
            bool allProductsArePositivePrice = products.All(p => p.Price > 0);
            Console.WriteLine("--- 10. Проверка: все ли продукты имеют цену > 0 (All) ---");
            Console.WriteLine($"Результат: {allProductsArePositivePrice}\n");

            // 11. Проверка наличия хотя бы одного (Метод расширения Any)
            Console.Write("11. Введите производителя для проверки наличия (e.g., Logitech, Microsoft): ");
            string manufacturerToCheck = Console.ReadLine();
            bool hasAnyFromManufacturer = products.Any(p => p.ManufacturerName == manufacturerToCheck);
            Console.WriteLine("--- 11. Проверка: есть ли хоть один продукт от указанного производителя (Any) ---");
            Console.WriteLine($"Результат для '{manufacturerToCheck}': {hasAnyFromManufacturer}\n");
        }
    }
}