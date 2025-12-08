using System;
using System.Threading.Tasks;

namespace Lab9
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Task 1: Dining Savages ===");
            Task1_Savages.Run(5, 3);

            Console.WriteLine("\n=== Task 2.1: Async/Await ===");
            await Task2_Async.RunAsync();

            Console.WriteLine("\n=== Task 2.2: AutoResetEvent ===");
            Task2_AutoReset.Run();

            Console.WriteLine("\nAll tasks finished. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}