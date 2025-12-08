using System;
using System.Threading;

namespace Lab9
{
    public class Task2_AutoReset
    {
        private static AutoResetEvent _event1 = new AutoResetEvent(true);
        private static AutoResetEvent _event2 = new AutoResetEvent(false);
        private static bool _running = true;

        public static void Run()
        {
            Thread t1 = new Thread(Worker1);
            Thread t2 = new Thread(Worker2);

            t1.Start();
            t2.Start();

            Thread.Sleep(3000);
            _running = false;

            _event1.Set();
            _event2.Set();
        }

        private static void Worker1()
        {
            while (_running)
            {
                _event1.WaitOne();
                if (!_running) break;

                Console.WriteLine("Thread 1: Ping");
                Thread.Sleep(300);
                _event2.Set();
            }
        }

        private static void Worker2()
        {
            while (_running)
            {
                _event2.WaitOne();
                if (!_running) break;

                Console.WriteLine("Thread 2: Pong");
                Thread.Sleep(300);
                _event1.Set();
            }
        }
    }
}