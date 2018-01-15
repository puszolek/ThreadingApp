using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingApp
{
    class Program
    {
        static int GLOBAL_VALUE = 0;
        static readonly object locker = new object();
        static bool done;

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";

            Thread t = new Thread(new ThreadStart(Increment));
            t.Name = "Uno";
            Thread t2 = new Thread(Increment);
            t2.Name = "Dos";

            Thread t3 = new Thread(new ThreadStart(() => Decrement(5)));
            t3.Name = "Tres";
            Thread t4 = new Thread(() => Decrement(10));
            t4.Name = "Quatro";

            t.Start(); 
            t2.Start();
            t3.Start();
            t4.Start();

            /*t.Join();
            t2.Join();
            t3.Join();
            t4.Join();*/


            t.Abort();
            t4.Abort();

            System.Console.WriteLine("Main thread at the end");
            System.Console.WriteLine("Global value at the end: " + GLOBAL_VALUE);
            System.Console.ReadKey();

        }

        public static void Increment()
        {            
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    System.Console.WriteLine(i + " " + Thread.CurrentThread.Name);
                    Thread.Sleep(10);

                    //lock (locker)
                    {
                            int tmp = GLOBAL_VALUE;
                            tmp += 1;
                            GLOBAL_VALUE = tmp;
                            System.Console.WriteLine(Thread.CurrentThread.Name + " Global value: " + GLOBAL_VALUE);
                    }

                }
                catch (ThreadAbortException ex)
                {
                    System.Console.WriteLine("Thread " + Thread.CurrentThread.Name + " aborted");
                    Thread.CurrentThread.Abort();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        public static void Decrement(int max)
        {
            for (int i = max; i >= 0; i--)
            {
                try
                { 
                    System.Console.WriteLine(i + " " + Thread.CurrentThread.Name);

                    //lock (locker)
                    {
                            int tmp = GLOBAL_VALUE;
                            tmp -= 1;
                            GLOBAL_VALUE = tmp;
                            System.Console.WriteLine(Thread.CurrentThread.Name + " Global value: " + GLOBAL_VALUE);
                    }

                }
                catch (ThreadAbortException ex)
                {
                    System.Console.WriteLine("Thread " + Thread.CurrentThread.Name + " aborted");
                    Thread.CurrentThread.Abort();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
