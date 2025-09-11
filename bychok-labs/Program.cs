
using System;
using System.Linq;
using static System.Math;

namespace Lab1 {
    class Program
    {
        #region Задание 1: Функции

        static double CalculateF(double x, double y)
        {
            double xy = x * y;
            if (xy < 3)
            {
                if (x - Pow(y, 3) == 0) throw new DivideByZeroException("Ошибка в f: деление на ноль (x - y^3 = 0).");
                return 5 / (x - Pow(y, 3));
            }
            if (xy >= 3 && xy <= 9)
            {
                if (5 * Pow(x, 3) - 2 * y == 0) throw new DivideByZeroException("Ошибка в f: деление на ноль (5x^3 - 2y = 0).");
                return (x + y - xy) / (5 * Pow(x, 3) - 2 * y);
            }
            if (xy <= 0) throw new ArgumentException("Ошибка в f: аргумент ln(xy) должен быть > 0.");
            return Log(xy);
        }

        static double CalculateG(double x, double y)
        {
            double xy = x * y;
            if (xy < 3)
            {
                if (x + y == 0) throw new DivideByZeroException("Ошибка в g: деление на ноль (x + y = 0).");
                return Atan((x - 1) / (x + y));
            }
            if (xy >= 3 && xy <= 9)
            {
                return 200 / (Pow(xy, 2) + Abs(3 * x));
            }
            return Sqrt(1 + x * x + y * y);
        }

        static void ExecuteTask1()
        {
            Console.WriteLine("--- Задание 1: Вычисление функций f и g ---");
            double xn = 0.5, xk = 2.5, h = 0.4;
            double yn = -1.0, yk = 1.0, t = 0.5;

            Console.WriteLine("+----------------+----------------+----------------+----------------+");
            Console.WriteLine("|       x        |       y        |       f        |       g        |");
            Console.WriteLine("+----------------+----------------+----------------+----------------+");

            for (double x = xn; x <= xk; x += h)
            {
                for (double y = yn; y <= yk; y += t)
                {
                    try
                    {
                        Console.WriteLine($"| {x,-14:F2} | {y,-14:F2} | {CalculateF(x, y),-14:F3} | {CalculateG(x, y),-14:F3} |");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"| {x,-14:F2} | {y,-14:F2} | {"Ошибка",-14} | {"Ошибка",-14} |");
                        Console.WriteLine($"| Ошибка: {ex.Message}");
                    }
                }
                Console.WriteLine("+----------------+----------------+----------------+----------------+");
            }
        }

        #endregion

        #region Задание 2: Векторы и Матрицы

        // --- Общие методы для ввода/вывода ---
        static int[] InputVector()
        {
            Console.WriteLine("Введите элементы вектора через пробел:");
            string[] parts = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int[] vector = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out vector[i]))
                {
                    throw new FormatException("Ошибка ввода. Вектор должен состоять из целых чисел.");
                }
            }
            return vector;
        }

        static void PrintVector(int[] vector, string title)
        {
            Console.WriteLine(title);
            Console.WriteLine(string.Join(" ", vector));
        }

        static int[,] InputMatrix()
        {
            Console.Write("Введите размерность квадратной матрицы n: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                throw new FormatException("Размерность должна быть положительным целым числом.");
            }

            int[,] matrix = new int[n, n];
            Console.WriteLine("Введите элементы матрицы построчно (числа через пробел):");
            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != n)
                {
                    throw new ArgumentException($"Ошибка: в строке {i + 1} должно быть {n} элементов.");
                }
                for (int j = 0; j < n; j++)
                {
                    if (!int.TryParse(parts[j], out matrix[i, j]))
                    {
                        throw new FormatException("Ошибка ввода. Матрица должна состоять из целых чисел.");
                    }
                }
            }
            return matrix;
        }

        static void PrintMatrix(int[,] matrix, string title)
        {
            Console.WriteLine(title);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j],5} ");
                }
                Console.WriteLine();
            }
        }

        // --- Задача 2.1: Номер первого минимального среди положительных после первого нуля ---
        static void SolveVectorTask1()
        {
            Console.WriteLine("\n--- 2.1: Номер минимального элемента после первого нуля ---");
            try
            {
                int[] vector = InputVector();
                PrintVector(vector, "Исходный вектор A:");

                int firstZeroIndex = Array.IndexOf(vector, 0);

                if (firstZeroIndex == -1)
                {
                    Console.WriteLine("Результат: в векторе нет элементов, равных нулю.");
                    return;
                }
                if (firstZeroIndex == vector.Length - 1)
                {
                    Console.WriteLine("Результат: ноль является последним элементом, справа от него нет элементов.");
                    return;
                }

                int min = int.MaxValue;
                int minIndex = -1;

                for (int i = firstZeroIndex + 1; i < vector.Length; i++)
                {
                    if (vector[i] > 0 && vector[i] < min)
                    {
                        min = vector[i];
                        minIndex = i;
                    }
                }

                if (minIndex != -1)
                {
                    Console.WriteLine($"Результат: номер первого минимального значения ({min}) среди положительных элементов правее первого нуля: {minIndex + 1}");
                }
                else
                {
                    Console.WriteLine("Результат: справа от первого нуля нет положительных элементов.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        // --- Задача 2.2: Вектор из минимальных элементов строк матрицы ---
        static void SolveMatrixTask1()
        {
            Console.WriteLine("\n--- 2.2: Вектор из минимальных элементов строк матрицы ---");
            try
            {
                int[,] matrix = InputMatrix();
                PrintMatrix(matrix, "Исходная матрица A:");

                int n = matrix.GetLength(0);
                int[] b = new int[n];

                for (int i = 0; i < n; i++)
                {
                    int minInRow = matrix[i, 0];
                    for (int j = 1; j < n; j++)
                    {
                        if (matrix[i, j] < minInRow)
                        {
                            minInRow = matrix[i, j];
                        }
                    }
                    b[i] = minInRow;
                }

                PrintVector(b, "Результирующий вектор b:");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        // --- Задача 2.3: Максимальная сумма элементов, параллельных главной диагонали ---
        static void SolveMatrixTask2()
        {
            Console.WriteLine("\n--- 2.3: Максимальная сумма на диагоналях ---");
            try
            {
                int[,] matrix = InputMatrix();
                PrintMatrix(matrix, "Исходная матрица A:");
                int n = matrix.GetLength(0);

                if (n == 0)
                {
                    Console.WriteLine("Результат: матрица пуста, сумма равна 0.");
                    return;
                }

                long maxSum = long.MinValue;

                // Диагонали над главной (включая главную)
                for (int k = 0; k < n; k++)
                {
                    long currentSum = 0;
                    for (int i = 0; i < n - k; i++)
                    {
                        currentSum += matrix[i, i + k];
                    }
                    if (currentSum > maxSum) maxSum = currentSum;
                }

                // Диагонали под главной
                for (int k = 1; k < n; k++)
                {
                    long currentSum = 0;
                    for (int i = 0; i < n - k; i++)
                    {
                        currentSum += matrix[i + k, i];
                    }
                    if (currentSum > maxSum) maxSum = currentSum;
                }

                Console.WriteLine($"Результат: максимальная сумма элементов, параллельных главной диагонали, равна: {maxSum}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        static void ExecuteTask2()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Задание 2: Выберите задачу ---");
                Console.WriteLine("1. Вектор: Номер минимального после нуля");
                Console.WriteLine("2. Матрица: Вектор из минимумов строк");
                Console.WriteLine("3. Матрица: Максимальная сумма на параллельных диагоналях");
                Console.WriteLine("4. Вернуться в главное меню");
                Console.Write("Ваш выбор: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        SolveVectorTask1();
                        break;
                    case "2":
                        SolveMatrixTask1();
                        break;
                    case "3":
                        SolveMatrixTask2();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт от 1 до 4.");
                        break;
                }
            }
        }

        #endregion

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- ГЛАВНОЕ МЕНЮ ---");
                Console.WriteLine("1. Задание 1 (Функции)");
                Console.WriteLine("2. Задание 2 (Векторы и Матрицы)");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите пункт меню: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ExecuteTask1();
                        break;
                    case "2":
                        ExecuteTask2();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт от 1 до 3.");
                        break;
                }
            }
            Console.WriteLine("Программа завершена.");
        }
    }
}