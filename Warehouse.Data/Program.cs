using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Data;

namespace Warehouse.ConsoleApp
{
    class Program
    {
        private static readonly StockRepository _repository = new StockRepository();
        private static List<StockReportItem> _lastQueryResult = new List<StockReportItem>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Управление складом =====");
                DisplayStock();

                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить запись");
                Console.WriteLine("2. Изменить количество");
                Console.WriteLine("3. Удалить запись");
                Console.WriteLine("4. Поиск и фильтрация");
                Console.WriteLine("5. Сортировка");
                Console.WriteLine("6. Создать отчеты в Word");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": AddStock(); break;
                    case "2": UpdateStock(); break;
                    case "3": DeleteStock(); break;
                    case "4": FilterStock(); break;
                    case "5": SortStock(); break;
                    case "6": GenerateReports(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void DisplayStock(List<StockReportItem> stockList = null)
        {
            if (stockList == null)
            {
                _lastQueryResult = _repository.GetStockReport();
                stockList = _lastQueryResult;
            }

            Console.WriteLine("\n--- Текущие запасы на складах ---");
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-30} {4,-10}", "ID", "Продукт", "Категория", "Склад", "Кол-во");
            Console.WriteLine(new string('-', 85));
            foreach (var item in stockList)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-30} {4,-10}", item.StockID, item.ProductName, item.Category, item.WarehouseLocation, item.Quantity);
            }
        }

        static int GetValidIntInput(string message)
        {
            int result;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Ошибка: Введите корректное число.");
                Console.Write(message);
            }
            return result;
        }

        static void AddStock()
        {
            try
            {
                var products = _repository.GetAllProducts();
                Console.WriteLine("\nВыберите продукт из списка:");
                products.ForEach(p => Console.WriteLine($"  {p.ProductID}. {p.Name}"));
                int productId = GetValidIntInput("ID продукта: ");

                var warehouses = _repository.GetAllWarehouses();
                Console.WriteLine("\nВыберите склад из списка:");
                warehouses.ForEach(w => Console.WriteLine($"  {w.WarehouseID}. {w.Location}"));
                int warehouseId = GetValidIntInput("ID склада: ");

                int quantity = GetValidIntInput("Введите количество: ");

                _repository.AddStockItem(productId, warehouseId, quantity);
                Console.WriteLine("Запись успешно добавлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void UpdateStock()
        {
            try
            {
                int stockId = GetValidIntInput("\nВведите ID записи для изменения: ");
                int newQuantity = GetValidIntInput("Введите новое количество: ");

                _repository.UpdateStockQuantity(stockId, newQuantity);
                Console.WriteLine("Количество успешно обновлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void DeleteStock()
        {
            try
            {
                int stockId = GetValidIntInput("\nВведите ID записи для удаления: ");

                Console.Write($"Вы уверены, что хотите удалить запись с ID {stockId}? (y/n): ");
                if (Console.ReadLine().Trim().ToLower() == "y")
                {
                    _repository.DeleteStockItem(stockId);
                    Console.WriteLine("Запись успешно удалена!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void FilterStock()
        {
            try
            {
                Console.WriteLine("\n--- Поиск и фильтрация ---");
                Console.Write("Введите название продукта (или оставьте пустым): ");
                string productName = Console.ReadLine();

                Console.Write("Введите расположение склада (или оставьте пустым): ");
                string warehouseLocation = Console.ReadLine();

                _lastQueryResult = _repository.FilterStock(productName, warehouseLocation);
                if (_lastQueryResult.Any())
                {
                    DisplayStock(_lastQueryResult);
                }
                else
                {
                    Console.WriteLine("\nПо вашему запросу ничего не найдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
            }
        }

        static void SortStock()
        {
            Console.WriteLine("\n--- Сортировка текущей выборки ---");
            Console.WriteLine("1. По названию продукта (А-Я)");
            Console.WriteLine("2. По названию продукта (Я-А)");
            Console.WriteLine("3. По количеству (возрастание)");
            Console.WriteLine("4. По количеству (убывание)");
            Console.Write("Ваш выбор: ");

            var sortedList = new List<StockReportItem>();

            switch (Console.ReadLine())
            {
                case "1": sortedList = _lastQueryResult.OrderBy(item => item.ProductName).ToList(); break;
                case "2": sortedList = _lastQueryResult.OrderByDescending(item => item.ProductName).ToList(); break;
                case "3": sortedList = _lastQueryResult.OrderBy(item => item.Quantity).ToList(); break;
                case "4": sortedList = _lastQueryResult.OrderByDescending(item => item.Quantity).ToList(); break;
                default: Console.WriteLine("Неверный ввод."); return;
            }
            DisplayStock(sortedList);
        }

        static void GenerateReports()
        {
            Console.WriteLine("\n--- Формирование отчетов ---");
            Console.WriteLine("1. Отчет по всем данным в БД");
            Console.WriteLine("2. Отчет по результатам последнего запроса/фильтрации");
            Console.WriteLine("3. Сводный отчет по складам (группировка + итог)");
            Console.Write("Ваш выбор: ");
            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        var allData = _repository.GetStockReport();
                        WordReportGenerator.CreateFullStockReport(allData, "Полный отчет по всем записям");
                        break;
                    case "2":
                        WordReportGenerator.CreateFullStockReport(_lastQueryResult, "Отчет по данным последнего запроса");
                        break;
                    case "3":
                        var groupedData = _repository.GetStockGroupedByWarehouse();
                        WordReportGenerator.CreateGroupedStockReport(groupedData);
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла ошибка при создании отчета: {ex.Message}");
            }
        }
    }
}