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
        public static int SinglThreadResult { get; private set; }

        /// <summary>
        /// Производит вычисление количества чисел, сумма цифр которых кратна последней цифре в однопоточном режиме в указанном диапазоне
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public static void SingleThreadCalculate(int startPoint, int endPoint)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} приступил к вычислениям. Диапазон: {startPoint} - {endPoint}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = startPoint; i < endPoint + 1; i++)
            {
                // переводим число в текстовый формат для того чтобы получить возможность обращаться к его цифрам (символам) по индексу
                string text = i.ToString();

                int sum = 0;
                // определеяем сумму цифр числа
                for (int j = 0; j < text.Length; j++)
                {
                    sum += text[j];
                }

                // если остаток от деления суммы цифр на последнюю цифру равен 0
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
