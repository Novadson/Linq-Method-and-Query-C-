
namespace LinqPractice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Task<int> result = LongProcess();

            ShortProcess();

            await MediumProcess();


            var val = await result; // wait untile get the return value

            Console.WriteLine("Result: {0}", val);

            Console.ReadKey();
        }

        static async Task<int> LongProcess()
        {
            Console.WriteLine("LongProcess Started");

            await Task.Delay(4000); // hold execution for 4 seconds

            Console.WriteLine("LongProcess Completed");

            return 10;
        }

        static async Task MediumProcess()
        {
            Console.WriteLine("Medium process started");
            await Task.Delay(5000);

            Console.WriteLine("Medium process started");
        }

        static void ShortProcess()
        {
            Console.WriteLine("ShortProcess Started");

            //do something here

            Console.WriteLine("ShortProcess Completed");
        }


        public static Task<int> GetIntTask(int num1, int num2) => Task.FromResult(num1 + num2);

        public static async Task<int> GetIntAsync(int num1, int num2)
        {
            return await Task.FromResult(num1 + num2);
        }

    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Runtime.Intrinsics.X86;
//using System.Diagnostics.Metrics;

//namespace LinqPractice
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            /*What is thread?= Thread in an execution path of the program,
//             * we can use multiple threads to perform different tasks og our progam at the same time.
//             * The current thread is running "main" thread using System.Thread
//            */

//            Thread mainThread = Thread.CurrentThread;

//            mainThread.Name = "This thread now called Not main";

//            Thread threadCountDown = new(CountDown);
//            Thread threadCountUp = new(CountUp);

//            //Thread threadCountUpWithParameter = new(() => CountUpWithParameter($"Count Up with parameter!"));
//            //Thread threadCountDownWithParameter = new(() => CountDownWithParameter("Count Up with parameter!"));
//            //threadCountUpWithParameter.Start();
//            //threadCountDownWithParameter.Start();

//            threadCountUp.Start();
//            threadCountDown.Start();

//            Console.WriteLine($"{mainThread.Name} is complete!");

//            Console.WriteLine();

//            Console.ReadKey();

//        }


//        public static void CountUp()
//        {
//            lock (typeof(Program))
//            {

//                for (int i = 0; i <= 10; i++)
//                {
//                    Console.WriteLine($"Timer #1 count up {i} seconds");
//                    Thread.Sleep(1000);
//                }
//                Console.WriteLine("Time #1 is completed.");
//            }


//        }

//        public static void CountDown()
//        {

//            for (int i = 10; i >= 0; i--)
//            {
//                Console.WriteLine($"Timer #2  count down  {i} seconds");
//                Thread.Sleep(1000);
//            }
//            Console.WriteLine("Time #2 is completed.");

//        }

//        public static void CountUpWithParameter(string name)// with parameter
//        {

//            for (int i = 0; i <= 10; i++)
//            {
//                Console.WriteLine($"Timer #3 count up {i} seconds wiht Parameter");
//                Thread.Sleep(1000);
//            }
//            Console.WriteLine("Time #1 is completed.");

//        }

//        public static void CountDownWithParameter(string name)// with parameter
//        {

//            for (int i = 10; i >= 0; i--)
//            {
//                Console.WriteLine($"Timer #4  count down  {i} seconds wiht Parameter");
//                Thread.Sleep(1000);
//            }
//            Console.WriteLine("Time #2 is completed.");

//        }
//    }
//}
/*
using System;
using System.Threading;

class Program
{
    private static readonly object lockObject = new object(); // Shared lock object

    static void Main(string[] args)
    {
        // Create two threads that share the same resource
        Thread thread1 = new(ThreadMethod1);
        Thread thread2 = new(ThreadMethod2);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Both threads have completed execution.");
        Console.ReadKey();
    }

    static void ThreadMethod1()
    {
        //lock (lockObject) // Synchronize access using the lock object
        //{
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread 1: {i}");
                Thread.Sleep(500); // Simulate work
            }
        //}
    }

    static void ThreadMethod2()
    {
        lock (lockObject) // Synchronize access using the lock object
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread 2: {i}");
                Thread.Sleep(500); // Simulate work
            }
        }
    }
}
*/