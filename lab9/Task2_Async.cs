using System;
using System.Threading.Tasks;

namespace Lab9
{
    public class Task2_Async
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("Main: Starting async operation...");
            await DoWorkAsync();
            Console.WriteLine("Main: Async operation completed.");
        }

        private static async Task DoWorkAsync()
        {
            Console.WriteLine("AsyncMethod: Work started.");
            await Task.Delay(2000);
            Console.WriteLine("AsyncMethod: Work finished after delay.");
        }
    }
}