using System;
using System.Collections.Generic;

namespace Lab2
{
    class Program
    {
        // 19. В функции Main демонстрируется использование всех разработанных элементов классов
        static void Main(string[] args)
        {
            Console.WriteLine("--- Лабораторная работа 2: Магазин канцтоваров ---\n");

            // 1, 2, 3, 7, 9, 10: Классы, конструкторы, свойства, инкапсуляция, наследование, переопределение
            Console.WriteLine("--- 1-3, 7, 9, 10: Создание объектов и базовые операции ---");
            Pen bluePen = new Pen("Ручка гелевая", 45.50m, "Синий");
            Notebook largeNotebook = new Notebook("Блокнот А4", 120m, 96);
            bluePen.DisplayInfo();
            largeNotebook.DisplayInfo();
            Console.WriteLine();

            // 4: Свойство с логикой (статическое свойство и валидация в конструкторе)
            Console.WriteLine("--- 4: Демонстрация свойств с логикой ---");
            Console.WriteLine($"Всего создано канцтоваров: {StationeryItem.TotalItemsCreated}");
            try
            {
                Notebook invalidNotebook = new Notebook("Брак", 10m, -50);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Поймано исключение при создании блокнота: {e.Message}");
            }
            Console.WriteLine();

            // 6, 9: Абстрактный класс нельзя создать напрямую
            Console.WriteLine("--- 6: Абстрактный класс ---");
            // StationeryItem item = new StationeryItem("test", 1m); // Эта строка вызовет ошибку компиляции
            Console.WriteLine("Экземпляр абстрактного класса StationeryItem создать нельзя.\n");

            // 8: Статический класс и статические члены (см. StationeryItem.TotalItemsCreated выше)

            // 11, 16: Агрегация и перегрузка операторов
            Console.WriteLine("--- 11, 16: Агрегация и перегрузка операторов ---");
            Shop myShop = new Shop("КанцПарк");
            Employee john = new Employee("Иван"); // Сотрудник существует независимо от магазина
            myShop.AddEmployee(john);
            myShop += bluePen;      // Используем перегруженный оператор +
            myShop += largeNotebook;
            myShop.PrintAllItems();
            Console.WriteLine();

            // 5: Индексатор
            Console.WriteLine("--- 5: Индексатор ---");
            Console.Write("Первый товар в магазине (через индексатор): ");
            myShop[0].DisplayInfo();
            Console.WriteLine();

            // 17: Композиция
            Console.WriteLine("--- 17: Композиция ---");
            Console.WriteLine($"Информация о внутреннем компоненте ручки: {bluePen.GetCartridgeInfo()}");
            Console.WriteLine("Объект 'Стержень' не может существовать без объекта 'Ручка'.\n");

            // 12: Обобщения (Generic-класс)
            Console.WriteLine("--- 12: Обобщенный класс Box<T> ---");
            Box<Notebook> notebookBox = new Box<Notebook>();
            notebookBox.Add(largeNotebook);
            // notebookBox.Add(bluePen); // Ошибка компиляции, т.к. коробка только для блокнотов
            Console.WriteLine();

            // 14: Наследование обобщений
            Console.WriteLine("--- 14: Наследование обобщений ---");
            PenBox<Pen> penBox = new PenBox<Pen>();
            penBox.Add(bluePen);
            penBox.GetPensByColor("Синий");
            Console.WriteLine();

            // 13: Обобщенный метод
            Console.WriteLine("--- 13: Обобщенный метод ---");
            Pen[] pensInShop = myShop.GetItemsOfType<Pen>();
            Console.WriteLine($"Найдено ручек в магазине: {pensInShop.Length}");
            foreach (var pen in pensInShop)
            {
                pen.DisplayInfo();
            }
            Console.WriteLine();

            // 18: Интерфейс
            Console.WriteLine("--- 18: Интерфейс ISellable ---");
            ISellable sellableItem = bluePen; // Ссылка типа интерфейса на объект класса
            sellableItem.Sell();
            Console.WriteLine();

            // 15: Метод расширения
            Console.WriteLine("--- 15: Метод расширения ---");
            Console.WriteLine($"Обычная цена блокнота: {largeNotebook.Price:C}");
            decimal discountedPrice = largeNotebook.GetDiscountedPrice(15); // Вызываем метод расширения
            Console.WriteLine($"Цена блокнота со скидкой 15%: {discountedPrice:C}");

            Console.WriteLine("\n--- Демонстрация всех пунктов завершена. ---");

            Console.WriteLine("\n--- 14: Улучшенная демонстрация наследования обобщений ---");
            PenBox<Pen> detailedPenBox = new PenBox<Pen>();
            detailedPenBox.Add(new Pen("Pilot", 150m, "Черный"));
            detailedPenBox.Add(new Pen("ErichKrause", 30m, "Синий"));
            detailedPenBox.Add(new Pen("Bic", 25m, "Синий"));

            string colorToSearch = "Синий";
            List<Pen> bluePens = detailedPenBox.GetPensByColor(colorToSearch);

            Console.WriteLine($"Найдены ручки с цветом '{colorToSearch}': {bluePens.Count} шт.");
            foreach (var pen in bluePens)
            {
                pen.DisplayInfo();
            }

        }
    }
}