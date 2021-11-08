using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DZ_16._2
{
    class MultyThreadsCalculator
    {
        public int FinalResult { get; set; }

        /// <summary>
        /// Number of threads
        /// </summary>
        private int ThreadsNum { get; set; } = 10;

        /// <summary>
        /// Calculates the number of numbers whose sum of digits is a multiple of the last digit in multithreaded mode
        /// </summary>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        public void Calculate(int startNum, int endNum)
        {
            // проверка корректности параметров
            if (startNum >= endNum)
            {
                Console.WriteLine("The starting point must be smaller than the end point. Сalculations are impossible.");
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // формируем список задач для одиночных параллельных потоков
            List<SingleTaskForMultiThreadCalculation> tasksList = new List<SingleTaskForMultiThreadCalculation>();

            // если интервал для вычислений меньше 1 миллиона то достаточно одного потока
            int threadsNum = endNum - startNum > 1_000_000 ? ThreadsNum : 1;

            for (int i = 0; i < threadsNum; i++)
            {
                tasksList.Add(new SingleTaskForMultiThreadCalculation());
            }

            // задаем интервалы (крайние точки) вычислений для каждого из потоков
            int interval = (endNum - startNum) / threadsNum;

            int currentEndNum = startNum;

            Func<int> func = () => currentEndNum += interval;

            List<Task> tasks = new List<Task>();

            foreach (var item in tasksList)
            {
                var task = Task.Run(() => item.Calculate(currentEndNum, func()));

                tasks.Add(task);

                Thread.Sleep(2);
            }

            Task.WaitAll(tasks.ToArray());

            //Thread.Sleep(500);

            // FinalResult = intervalsList.Select(x => x.Result).Aggregate((x, y) => x + y);  // применение linq негативно сказалось на скорости вычислений
            // и снизило стабильность, поэтому для суммирования результатов всех потоков используем простой цикл:

            for (int i = 0; i < threadsNum; i++)
            {
                FinalResult += tasksList[i].Result;
            }

            // если последняя цифра последнего числа равна 0 (то есть 1 она уже не станет), то вычитаем 1 так как мы ранее добавили 1 применительно ко всем числам 
            if (endNum % 10 == 0)
            {
                FinalResult -= 1;
            }

            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Result in multi-threaded mode {String.Format("{0:### ### ###}", FinalResult)}, " +
                $"time {stopwatch.ElapsedMilliseconds} milliseconds\n");
            Console.ResetColor();
        }
    }
}
