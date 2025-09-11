using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Lab4
{
    class Program
    {
        //===================================================================
        #region Задание 1: Строки (симметричные слова)

        /// <summary>
        /// Логика поиска симметричных слов нечетной длины.
        /// </summary>
        /// <param name="inputText">Входной текст.</param>
        /// <returns>Список найденных слов.</returns>
        static List<string> FindOddLengthPalindromes(string inputText)
        {
            // Используем Regex для разделения текста на слова, включая дефисы
            var words = Regex.Split(inputText.ToLower(), @"\W+").Where(w => !string.IsNullOrEmpty(w));
            var palindromes = new List<string>();

            foreach (var word in words)
            {
                if (word.Length % 2 == 0) continue; // Пропускаем слова четной длины

                string reversedWord = new string(word.Reverse().ToArray());
                if (word == reversedWord)
                {
                    palindromes.Add(word);
                }
            }
            return palindromes;
        }

        /// <summary>
        /// Обработчик для Задания 1: получает данные с клавиатуры и выводит на экран.
        /// </summary>
        static void HandleSymmetricWordsTask()
        {
            Console.WriteLine("\n--- 1. Поиск симметричных слов нечетной длины ---");
            Console.WriteLine("Введите текст:");
            string userInput = Console.ReadLine();

            List<string> result = FindOddLengthPalindromes(userInput);

            Console.WriteLine("Найденные симметричные слова нечетной длины:");
            if (result.Any())
            {
                Console.WriteLine(string.Join(", ", result));
            }
            else
            {
                Console.WriteLine("Таких слов не найдено.");
            }
        }

        #endregion
        //===================================================================

        //===================================================================
        #region Задание 2: Текстовый файл (слова с 4 гласными)

        /// <summary>
        /// Логика преобразования строк: слова с <= 4 гласными переводятся в верхний регистр.
        /// </summary>
        /// <param name="lines">Массив строк для обработки.</param>
        /// <returns>Массив обработанных строк.</returns>
        static string[] CapitalizeWordsWithFewVowels(string[] lines)
        {
            string vowels = "aeiouyаеёиоуыэюя";
            var processedLines = new List<string>();

            foreach (var line in lines)
            {
                var words = line.Split(' ');
                var processedWords = new List<string>();

                foreach (var word in words)
                {
                    // Подсчитываем гласные в слове
                    int vowelCount = word.ToLower().Count(c => vowels.Contains(c));

                    if (vowelCount <= 4 && !string.IsNullOrWhiteSpace(word))
                    {
                        processedWords.Add(word.ToUpper());
                    }
                    else
                    {
                        processedWords.Add(word);
                    }
                }
                processedLines.Add(string.Join(" ", processedWords));
            }
            return processedLines.ToArray();
        }

        /// <summary>
        /// Обработчик для Задания 2: работает с файлами.
        /// </summary>
        static void HandleVowelCountFileTask()
        {
            Console.WriteLine("\n--- 2. Обработка текстового файла ---");
            string inputFile = "input.txt";
            string outputFile = "output.txt";

            // Создаем исходный файл для демонстрации
            string[] initialText = {
                "Красивая иллюстрация и замечательное повествование.",
                "Программирование это увлекательный процесс.",
                "Аутентификация и авторизация - ключевые понятия безопасности."
            };
            File.WriteAllLines(inputFile, initialText, Encoding.UTF8);
            Console.WriteLine($"Создан демонстрационный файл '{inputFile}'.");

            // Чтение, обработка, запись
            string[] linesToProcess = File.ReadAllLines(inputFile, Encoding.UTF8);
            string[] result = CapitalizeWordsWithFewVowels(linesToProcess);
            File.WriteAllLines(outputFile, result, Encoding.UTF8);

            Console.WriteLine($"Файл обработан. Результат записан в '{outputFile}'.");
        }

        #endregion
        //===================================================================

        //===================================================================
        #region Задание 3: JSON файл (база данных магазина)

        /// <summary>
        /// Логика создания JSON файла из коллекции объектов.
        /// </summary>
        /// <param name="products">Коллекция продуктов.</param>
        /// <param name="filePath">Путь к файлу.</param>
        static void CreateProductsJsonFile(List<Product> products, string filePath)
        {
            // Настройки для красивого вывода JSON и поддержки кириллицы
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };
            string jsonString = JsonSerializer.Serialize(products, options);
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Логика чтения JSON и распределения продуктов по файлам производителей.
        /// </summary>
        /// <param name="jsonFilePath">Путь к исходному JSON файлу.</param>
        static void DistributeProductsByManufacturer(string jsonFilePath)
        {
            // 1. Создание директории на рабочем столе
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string targetDirectory = Path.Combine(desktopPath, "Products");
            Directory.CreateDirectory(targetDirectory); // Создаст, если не существует

            // 2. Чтение и десериализация JSON файла
            string jsonContent = File.ReadAllText(jsonFilePath);
            List<Product> products = JsonSerializer.Deserialize<List<Product>>(jsonContent);

            // 3. Группировка продуктов по производителю с помощью LINQ
            var groupedByManufacturer = products.GroupBy(p => p.Manufacturer);

            // 4. Распределение по файлам
            foreach (var group in groupedByManufacturer)
            {
                string manufacturerName = group.Key;
                string manufacturerFilePath = Path.Combine(targetDirectory, $"{manufacturerName}.txt");

                // Формируем строки для записи в файл "Наименование, цена"
                var linesToWrite = group.Select(p => $"{p.Title}, {p.Price}");

                File.WriteAllLines(manufacturerFilePath, linesToWrite, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Обработчик для Задания 3.
        /// </summary>
        static void HandleJsonTask()
        {
            Console.WriteLine("\n--- 3. Работа с JSON файлом ---");
            string jsonFile = "database.json";

            // --- Шаг 1: Создание файла ---
            var productsCollection = new List<Product>
            {
                new Product { Title = "Молоко 'Простоквашино'", Manufacturer = "Danone", Price = 85.5 },
                new Product { Title = "Йогурт 'Activia'", Manufacturer = "Danone", Price = 55.0 },
                new Product { Title = "Сок 'J7'", Manufacturer = "PepsiCo", Price = 120.0 },
                new Product { Title = "Газировка 'Pepsi'", Manufacturer = "PepsiCo", Price = 70.0 },
                new Product { Title = "Шоколад 'Alpen Gold'", Manufacturer = "Mondelez", Price = 99.9 }
            };
            CreateProductsJsonFile(productsCollection, jsonFile);
            Console.WriteLine($"Создан демонстрационный JSON файл '{jsonFile}'.");

            // --- Шаг 2: Чтение и обработка ---
            DistributeProductsByManufacturer(jsonFile);
            Console.WriteLine("Продукты распределены по файлам производителей в папке 'Products' на вашем рабочем столе.");
        }


        #endregion
        //===================================================================

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; 

            while (true)
            {
                Console.WriteLine("\n===== Главное меню Лабораторной работы 4 =====");
                Console.WriteLine("1. Строки (найти симметричные слова)");
                Console.WriteLine("2. Текстовый файл (изменить регистр слов)");
                Console.WriteLine("3. Json файл (распределить продукты)");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите пункт меню: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        HandleSymmetricWordsTask();
                        break;
                    case "2":
                        HandleVowelCountFileTask();
                        break;
                    case "3":
                        HandleJsonTask();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод, попробуйте снова.");
                        break;
                }
            }
        }
    }
}