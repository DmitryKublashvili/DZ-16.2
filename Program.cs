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
            /////////////////////   Многопоточный режим \\\\\\\\\\\\\\\\\\\\\\\\\\
            Console.WriteLine("Производятся вычисления в многопоточном режиме");

            MultyThreadsCalculator calculator = new MultyThreadsCalculator(1_000_000_000, 2_000_000_000);

            calculator.Calculate();

            /////////////////////   Однопоточный режим \\\\\\\\\\\\\\\\\\\\\\\\\\

            Console.WriteLine("Производятся вычисления в однопоточном режиме");

            SingleThreadCalculate(1_000_000_000, 2_000_000_000);

            Console.ReadKey();
        }
    }
}
