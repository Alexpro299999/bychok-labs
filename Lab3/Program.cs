using System;
using System.Linq;

namespace Lab3
{
    class Program
    {
        //===================================================================
        #region Задание 1: Делегаты для математических функций

        delegate double MathFunction(double x, double y);

       static double F(double x, double y) => x - y + 8;
        static double G(double x, double y) => x * y;

        static void ExecuteTask1()
        {
            Console.WriteLine("\n--- Задание 1: Использование делегатов ---");

            MathFunction func = null;

            Console.Write("Введите x: ");
            double x = double.Parse(Console.ReadLine());
            Console.Write("Введите y: ");
            double y = double.Parse(Console.ReadLine());

            Console.Write("Выберите функцию для вычисления z (1 - для f(x,y), 2 - для g(x,y)): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    func = F;
                    break;
                case "2":
                    func = G;
                    break;
                default:
                    Console.WriteLine("Ошибка: неверный выбор.");
                    return;
            }

            // Вычисляем z, используя делегат 'func'.
            // Делегат сам вызовет тот метод, который ему был присвоен.
            // z = func(3*x, y) + func(x, y+5);
            double z = func(3 * x, y) + func(x, y + 5);

            Console.WriteLine($"Результат: z = {z}");
        }

        #endregion
        //===================================================================

        //===================================================================
        #region Задание 2: Лямбда-выражения для обработки вектора

       static int SumElementsByCondition(int[] vector, Predicate<int> condition)
        {
            int sum = 0;
            foreach (int item in vector)
            {
                if (condition(item))
                {
                    sum += item;
                }
            }
            return sum;
        }

        static int CountElementsByCondition(int[] vector, Predicate<int> condition)
        {
            int count = 0;
            foreach (int item in vector)
            {
                if (condition(item))
                {
                    count++;
                }
            }
            return count;
        }

        static void ExecuteTask2()
        {
            Console.WriteLine("\n--- Задание 2: Использование лямбда-выражений ---");
            Console.WriteLine("Введите элементы вектора через пробел:");
            int[] vector = Console.ReadLine().Split().Select(int.Parse).ToArray();

            Console.Write("Выберите действие (1 - подсчет суммы кратных 3, 2 - подсчет количества нечетных): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                     int sum = SumElementsByCondition(vector, element => element % 3 == 0);
                    Console.WriteLine($"Сумма элементов, кратных 3: {sum}");
                    break;
                case "2":
                    int count = CountElementsByCondition(vector, element => element % 2 != 0);
                    Console.WriteLine($"Количество нечетных элементов: {count}");
                    break;
                default:
                    Console.WriteLine("Ошибка: неверный выбор.");
                    break;
            }
        }

        #endregion
        //===================================================================

        //===================================================================
        #region Задание 3: События и пользовательские исключения

        public class VectorOperationManager
        {
            public event Action<int[]> SumRequested;
            public event Action<int[]> CountRequested;

            public void RaiseEventByChoice(string choice, int[] vector)
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Инициируется событие SumRequested...");
                        SumRequested?.Invoke(vector);
                        break;
                    case "2":
                        Console.WriteLine("Инициируется событие CountRequested...");
                        CountRequested?.Invoke(vector);
                        break;
                    default:
                        throw new InvalidInputException("Неверный выбор операции. Введите 1 или 2.");
                }
            }
        }

        static void ExecuteTask3()
        {
            Console.WriteLine("\n--- Задание 3: Использование событий и исключений ---");
            Console.WriteLine("Введите элементы вектора через пробел:");
            int[] vector = Console.ReadLine().Split().Select(int.Parse).ToArray();

            var eventManager = new VectorOperationManager();

            eventManager.SumRequested += (vec) =>
            {
                int sum = SumElementsByCondition(vec, e => e % 3 == 0);
                Console.WriteLine($"[Обработчик события] Сумма элементов, кратных 3: {sum}");
            };

            eventManager.CountRequested += (vec) =>
            {
                int count = CountElementsByCondition(vec, e => e % 2 != 0);
                Console.WriteLine($"[Обработчик события] Количество нечетных элементов: {count}");
            };

            try
            {
                Console.Write("Выберите действие (1 - сумма кратных 3, 2 - количество нечетных): ");
                string choice = Console.ReadLine();

                eventManager.RaiseEventByChoice(choice, vector);
            }
            catch (InvalidInputException ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка пользовательского ввода: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("--- Блок finally выполнен. Проверка ввода завершена. ---");
            }
        }

        #endregion
        //===================================================================

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== Главное меню Лабораторной работы 3 =====");
                Console.WriteLine("1. Задание 1 (Делегаты)");
                Console.WriteLine("2. Задание 2 (Лямбда-выражения)");
                Console.WriteLine("3. Задание 3 (События и исключения)");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите задание: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ExecuteTask1();
                        break;
                    case "2":
                        ExecuteTask2();
                        break;
                    case "3":
                        ExecuteTask3();
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