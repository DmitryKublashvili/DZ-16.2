using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DZ_16._2.SingleThreadCalculator;

namespace DZ_16._2
{
    class Program
    {
        static void Main(string[] args)
        {
            MultyThreadsCalculator calculator = new MultyThreadsCalculator();

            Task task = new Task(CallThreadPool);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            task.Start();

            task.Wait();

            stopwatch.Stop();

            task.Dispose();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nРезультат вычислений в многопоточном режиме:\nКоличество чисел {MultyThreadsCalculator.Result}, затрачено времени {stopwatch.Elapsed}\n");
            Console.ResetColor();

            Console.WriteLine("Для вычисления в однопоточном режиме и сравнения результатов нажмите любую клавишу");
            Console.ReadKey();

            Points points = new Points(1000000000, 2000000000);
            SingleThreadCalculate(points);

            Console.ReadKey();
        }

        public static void CallThreadPool()
        {
            while (!MultyThreadsCalculator.IsCalculationFinished)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MultyThreadsCalculator.Calculate));
                Thread.Sleep(200);
            }

        }

    }
}
