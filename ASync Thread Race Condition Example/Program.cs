using System;
using System.Collections.Generic;
using System.Threading;

namespace ASync_Thread_Race_Condition_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            WrongWay();
            CorrectWay();
        }

        static void WrongWay()
        {
            Console.WriteLine("This is the WRONG way to use operations that may cause race conditions:");
            Console.WriteLine();
            var threads = new List<Thread>();
            int num = 0;

            for (int i = 0; i < 4; i++)
            {
                var thread = new Thread(() =>
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        num++;
                        /*
                         * ++ operation steps:
                         * 1. get num value to temporary variable (temp)
                         * 2. add 1 to tmp
                         * 3. set tmp value to num
                         */
                    }

                    Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} finished.");
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join(); //make sure that all threads are done before going forward
            }

            Console.WriteLine();
            Console.WriteLine("The result is: " + num); //ochakvaniq rezultat e 400000, no ne se poluchava zaradi thread race condition problema

            Console.WriteLine();
            Console.WriteLine("====================================================================");
            Console.WriteLine();
        }

        static void CorrectWay()
        {
            Console.WriteLine("This is the CORRECT way to use operations that may cause race conditions:");
            Console.WriteLine();

            var lockObj = new object();
            var threads = new List<Thread>();
            int num = 0;

            for (int i = 0; i < 4; i++)
            {
                var thread = new Thread(() =>
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        lock (lockObj)
                        {
                            num++;
                            /*
                            * ++ operation steps:
                            * 1. get num value to temporary variable (temp)
                            * 2. add 1 to tmp
                            * 3. set tmp value to num
                            */
                        }
                    }

                    Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId} finished.");
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join(); //make sure that all threads are done before going forward
            }

            Console.WriteLine();
            Console.WriteLine("The result is: " + num); //veche rezultata e 400000 zashtoto chrez zakliuchaneto na obekta nishkite se izchakvat edna druga i garantirat reshenieto na race condition problema

            Console.WriteLine();
        }
    }
}