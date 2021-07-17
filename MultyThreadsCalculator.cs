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
        /// <summary>
        /// Результат вычислений
        /// </summary>
        public int Result { get; private set; }

        /// <summary>
        /// Число, начиная с которого поток начинает свои вычисления
        /// </summary>
        private int StartPoint { get; set; }

        /// <summary>
        /// Число, на котором поток заканчивает свои вычисления
        /// </summary>
        private int FinishPoint { get; set; }

        /// <summary>
        /// Нижняя граница диапазона
        /// </summary>
        private int LowLimit { get; set; }

        /// <summary>
        /// Верхняя граница диапазона
        /// </summary>
        private int HiLimit { get; set; }

        public bool IsCalculationFinished { get; set; }

        // объект, используемый для блокировки разделяемых ресурсов
        public object objectToLock; 

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="lowLimit"></param>
        /// <param name="hiLimit"></param>
        public MultyThreadsCalculator(int lowLimit, int hiLimit)
        {
            LowLimit = lowLimit;
            HiLimit = hiLimit;
            Result = 0;
            StartPoint = LowLimit;
            IsCalculationFinished = false;
            objectToLock = new object();
        }

        /// <summary>
        /// Производит вычисление количества чисел, сумма цифр которых кратна последней цифре в многопоточном режиме
        /// </summary>
        public void Calculate()
        {
            Task task = new Task(CallThreadPool);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            task.Start();

            task.Wait();

            stopwatch.Stop();

            task.Dispose();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nРезультат вычислений в многопоточном режиме:\nКоличество чисел {Result}, затрачено времени {stopwatch.Elapsed}\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Вызывает пулл потоков 
        /// </summary>
        private void CallThreadPool()
        {
            while (!IsCalculationFinished)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadCalculation));
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// Выполняет вычисления для отдельного потока
        /// </summary>
        /// <param name="o"></param>
        private void ThreadCalculation(Object o)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // получение интервала для вычислений
            Points points = GetPoints();

            // проверка выхода за пределы лимитов
            if (points.startPoint >= HiLimit)
            {
                return;
            }

            int currentStartPoint = points.startPoint;
            int currentFinishPoint = points.endPoint;

            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} приступил к вычислениям. Диапазон: {currentStartPoint} - {currentFinishPoint}");

            Thread.Sleep(100);

            // собственный результат потока
            int ownThreadResult = 0;

            for (int i = currentStartPoint; i < currentFinishPoint+1; i++)
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
                    AddResult();
                    ownThreadResult++;
                }
            }

            Console.WriteLine($"\nПоток {Thread.CurrentThread.ManagedThreadId} завершил вычисления. " +
                $"\nКоличество чисел (в диапазоне): {ownThreadResult}, время выполнения: {stopwatch.Elapsed}");

            // установление факта завершения вычислений
            if (points.endPoint >= HiLimit)
            {
                IsCalculationFinished = true;
            }
        }

        /// <summary>
        /// Возващает точки (числа) в пределах которых поток производит вычисления
        /// </summary>
        /// <returns></returns>
        private Points GetPoints()
        {
            lock (objectToLock)
            {
                FinishPoint = (HiLimit - 100_000_000) > StartPoint ? (StartPoint + 99_999_999) : HiLimit;

                Points points = new Points(StartPoint, FinishPoint);

                // устанавливаем значение стартового числа для следующего потока
                StartPoint = FinishPoint + 1;

                return points;
            }
        }

        /// <summary>
        /// Добавляет к результату единицу
        /// </summary>
        private void AddResult()
        {
            // блокировка разделяемого ресурса
            lock (objectToLock)
            {
                Result++;
            }
        }
    }
}
