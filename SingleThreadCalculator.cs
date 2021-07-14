using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DZ_16._2
{
    public static class SingleThreadCalculator
    {
        public static int SinglThreadResult { get; set; }

        public static void SingleThreadCalculate(Object points)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} приступил к вычислениям. Диапазон: {(points as Points).startPoint} - {(points as Points).endPoint}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = (points as Points).startPoint; i < (points as Points).endPoint + 1; i++)
            {
                string text = i.ToString();

                int sum = 0;

                for (int j = 0; j < text.Length; j++)
                {
                    sum += text[j];
                }

                if (sum % text[text.Length - 1] == 0)
                {
                    SinglThreadResult++;
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nПоток {Thread.CurrentThread.ManagedThreadId} завершил вычисления. \n" +
                $"Количество чисел: {SinglThreadResult}, время выполнения: {stopwatch.Elapsed}");
            Console.ResetColor();
        }



    }
}
