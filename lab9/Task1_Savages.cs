using System;
using System.Threading;

namespace Lab9
{
    public class Task1_Savages
    {
        private static int _servings;
        private static int _potCapacity;
        private static Semaphore _mutex;
        private static Semaphore _emptyPot;
        private static Semaphore _fullPot;
        private static volatile bool _isRunning = true; 

        public static void Run(int n, int m)
        {
            _potCapacity = m;
            _servings = m;
            _isRunning = true;
            _mutex = new Semaphore(1, 1);
            _emptyPot = new Semaphore(0, n);
            _fullPot = new Semaphore(0, 1);

            Thread cookThread = new Thread(Cook);
            cookThread.Start();

            Thread[] savages = new Thread[n];
            for (int i = 0; i < n; i++)
            {
                savages[i] = new Thread(Savage);
                savages[i].Name = $"Savage {i + 1}";
                savages[i].Start();
            }

            Thread.Sleep(2000);

            Console.WriteLine("\n--- Stopping Task 1 (waiting for threads to finish) ---\n");
            _isRunning = false;

            try { _mutex.Release(); } catch (SemaphoreFullException) { }
            try { _emptyPot.Release(n); } catch (SemaphoreFullException) { }
            try { _fullPot.Release(); } catch (SemaphoreFullException) { }

            cookThread.Join();
            for (int i = 0; i < n; i++)
            {
                savages[i].Join();
            }
        }

        private static void Savage()
        {
            while (_isRunning)
            {
                _mutex.WaitOne();

                if (!_isRunning)
                {
                    SafeRelease(_mutex);
                    break;
                }

                if (_servings == 0)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} sees empty pot and wakes cook.");
                    SafeRelease(_emptyPot);
                    _fullPot.WaitOne();
                }

                if (!_isRunning)
                {
                    SafeRelease(_mutex);
                    break;
                }

                _servings--;
                Console.WriteLine($"{Thread.CurrentThread.Name} eats. Servings left: {_servings}");

                SafeRelease(_mutex);

                Thread.Sleep(100);
            }
        }

        private static void Cook()
        {
            while (_isRunning)
            {
                _emptyPot.WaitOne();

                if (!_isRunning) break;

                Console.WriteLine("Cook is refilling the pot...");
                Thread.Sleep(500);
                _servings = _potCapacity;
                Console.WriteLine("Pot is full.");

                SafeRelease(_fullPot);
            }
        }

        private static void SafeRelease(Semaphore sem)
        {
            try
            {
                sem.Release();
            }
            catch (SemaphoreFullException)
            {
            }
        }
    }
}