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
            //////////////////// Multi-threaded calculations \\\\\\\\\\\\\\\\\\\\

            Console.WriteLine("Multi-threaded calculations...\n");

            new MultyThreadsCalculator().Calculate(1_000_000_000, 2_000_000_000);


            //////////////////// Single-thread calculations \\\\\\\\\\\\\\\\\\\\

            Console.WriteLine("Single-threaded calculations...\n");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var singleThreadCalculator = new SingleThreadCalculator();

            singleThreadCalculator.Calculate(1_000_000_000, 2_000_000_000);

            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Result in single-thread mode {String.Format("{0:### ### ###}", singleThreadCalculator.Result)}, " +
                $"time {stopwatch.ElapsedMilliseconds} milliseconds\n");
            Console.ResetColor();

            Console.WriteLine("Calculations complete\n");

            Console.ReadKey();
        }
    }
}
