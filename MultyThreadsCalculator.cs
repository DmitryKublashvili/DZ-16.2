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
        public static int Result { get; set; }

        public static int StartPoint { get; set; }

        public static int FinishPoint { get; set; }

        public TimeSpan RunningTime { get; set; }

        public const int LowLimit = 1000000000;

        public const int HiLimit = 2000000000;

        public static bool IsCalculationFinished { get; set; }

        public static object o = new object();

        static MultyThreadsCalculator()
        {
            Result = 0;
            StartPoint = LowLimit;
            IsCalculationFinished = false;
        }

        public static Points GetPoints()
        {
            lock (o)
            {
                FinishPoint = (HiLimit - 100000000) > StartPoint ? (StartPoint + 99999999) : HiLimit;

                Points points = new Points(StartPoint, FinishPoint);

                StartPoint = FinishPoint + 1;

                return points;
            }
        }

        public static void Calculate(Object o)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Points points = GetPoints();

            if (points.startPoint >= HiLimit)
            {
                return;
            }

            int currentStartPoint = points.startPoint;
            int currentFinishPoint = points.endPoint;

            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} приступил к вычислениям. Диапазон: {currentStartPoint} - {currentFinishPoint}");

            Thread.Sleep(5000);

            int nums = 0;

            for (int i = currentStartPoint; i < currentFinishPoint+1; i++)
            {
                string text = i.ToString();

                int sum = 0;

                for (int j = 0; j < text.Length; j++)
                {
                    sum += text[j];
                }

                if (sum % text[text.Length - 1] == 0)
                {
                    AddResult();
                    nums++;
                }
            }

            Console.WriteLine($"\nПоток {Thread.CurrentThread.ManagedThreadId} завершил вычисления. \nКоличество чисел (в диапазоне): {nums}, время выполнения: {stopwatch.Elapsed}");

            if (points.endPoint >= HiLimit)
            {
                IsCalculationFinished = true;
            }
        }


        private static void AddResult()
        {
            lock (o)
            {
                Result++;
            }
        }
    }
}
